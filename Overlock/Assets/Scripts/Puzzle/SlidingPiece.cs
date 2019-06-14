using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPiece : Piece
{
    [SerializeField]
    private Vector3 railStart;
    public Vector3 GetRailStart() { return railStart; }
    public void SetRailStart(Vector3 _railStart) { railStart = _railStart; }
    [SerializeField]
    private Vector3 railEnd;
    public Vector3 GetRailEnd() { return railEnd; }
    public void SetRailEnd(Vector3 _railEnd) { railEnd = _railEnd; }


    private Vector3 railVector;
    private Vector3 normalizedRailVector;
    private float distanceAlongRail;

    private float initialInteractionDistanceAlongRail;
    private float interactionDistanceAlongRail;
    private float initialDistanceAlongRail;


    protected override void Awake()
    {
        base.Awake();

        // Check the rail start and end and warn the player if there's a problem
        if (railStart.x != railEnd.x &&
            railStart.y != railEnd.y &&
            Mathf.Abs(railEnd.x - railStart.x) != Mathf.Abs(railEnd.y - railStart.y))
        { Debug.Log("One of the sliding piece rails is not vertical, horizontal or diagonal! This will cause problems!"); }
        if (railStart == railEnd)
        { Debug.Log("One of the sliding piece rails has length 0!"); }

        railVector = railEnd - railStart;
        normalizedRailVector = railVector.normalized;
        distanceAlongRail = PositionToDistanceAlongRail(transform.localPosition);
        distanceAlongRail = GetNearestStopPoint(distanceAlongRail);
        ClampDistanceAlongRail();
        ApplyDistanceAlongRail();

        SetCoOrds(Vector2Int.RoundToInt(transform.localPosition));
    }


    // Because movement bounds are relative to the piece, they become incorrect as soon as the piece is moved. I might try fix this, possibly by changing all movement bounds to use puzzle space.
    protected override void CalculateMovementBounds()
    {
        if (!shapeBoundsCached) CalculateShapeBounds();
        Vector2Int railStartLocalPos = Vector2Int.RoundToInt(railStart) - GetCoOrds();
        Vector2Int railEndLocalPos = Vector2Int.RoundToInt(railEnd) - GetCoOrds();
        movementLowerBounds = Vector2Int.Min(railStartLocalPos + shapeLowerBounds, railEndLocalPos + shapeLowerBounds);
        movementUpperBounds = Vector2Int.Max(railStartLocalPos + shapeUpperBounds, railEndLocalPos + shapeUpperBounds);
    }


    public override void StartInteraction(Vector3 position)
    {
        base.StartInteraction(position);

        initialInteractionDistanceAlongRail = PositionToDistanceAlongRail(position);
        initialDistanceAlongRail = distanceAlongRail;

        isBusy = true;
    }


    public override void ContinueInteraction(Vector3 position)
    {
        base.ContinueInteraction(position);

        interactionDistanceAlongRail = PositionToDistanceAlongRail(position);
        distanceAlongRail = initialDistanceAlongRail + interactionDistanceAlongRail - initialInteractionDistanceAlongRail;
        ClampDistanceAlongRail();
        ApplyDistanceAlongRail();

        Vector2Int newCoOrds = Vector2Int.RoundToInt(railStart + normalizedRailVector * GetNearestStopPoint(distanceAlongRail));
        if (newCoOrds != GetCoOrds())
        {
            ModifyOverlaps(-1);
            SetCoOrds(newCoOrds);
            ModifyOverlaps(1);
            puzzle.UpdateCrosses();
        }
    }


    public override void EndInteraction()
    {
        base.EndInteraction();

        float nearestStopPoint = GetNearestStopPoint(distanceAlongRail);
        if (distanceAlongRail == nearestStopPoint)
        {
            isBusy = false;
            puzzle.CheckIfSolved();
        }
        else
        {
            StartCoroutine(SlideToDistance(nearestStopPoint));
        }
    }


    private IEnumerator SlideToDistance(float distance)
    {
        float initialDistance = distanceAlongRail;
        float finalDistance = distance;

        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / 0.1f;
            if (f > 1) f = 1;

            distanceAlongRail = Mathf.SmoothStep(initialDistance, finalDistance, f);
            ApplyDistanceAlongRail();

            yield return null;
        }

        isBusy = false;
        puzzle.CheckIfSolved();
    }


    private void ClampDistanceAlongRail()
    {
        distanceAlongRail = Mathf.Clamp(distanceAlongRail, 0, railVector.magnitude);
    }


    private void ApplyDistanceAlongRail()
    {
        transform.localPosition = railStart + distanceAlongRail * normalizedRailVector;
    }


    private float PositionToDistanceAlongRail(Vector3 position)
    {
        return Vector3.Dot(position - railStart, normalizedRailVector);
    }


    private float GetNearestStopPoint(float distance)
    {
        return (railVector.x == 0 || railVector.y == 0) ?
            Mathf.Round(distance) :
            1.414f * Mathf.Round(distance / 1.414f);
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(railStart, railEnd);
    }
}
