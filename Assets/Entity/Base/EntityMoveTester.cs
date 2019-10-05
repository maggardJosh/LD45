using System;
using UnityEngine;

public class EntityMoveTester
{
    IEntityDebugLogger _logger;
    private MoveResult _result;
    private BoxCollider2D _collider;
    private LayerMask _collisionMask;

    public EntityMoveTester(IEntityDebugLogger logger, BoxCollider2D bCollider, LayerMask collisionMask)
    {
        _logger = logger;
        _collider = bCollider;
        _collisionMask = collisionMask;
    }

    public MoveResult GetMoveResult(Vector3 startPos, Vector3 amount)
    {
        _result = new MoveResult
        {
            newPos = startPos
        };

        if (Mathf.Abs(amount.x) > 0)
            TryMoveHorizontal(amount);

        if (Mathf.Abs(amount.y) > 0)
            TryMoveVertical(amount);

        return _result;
    }

    public MoveResult GetHorizontalRaycastFromCenter(Vector3 pos)
    {
        _result = new MoveResult
        {
            newPos = pos
        };

        TryRaycastHorizontal(Vector3.left);
        TryRaycastHorizontal(Vector3.right);

        return _result;
    }

    private void TryRaycastHorizontal(Vector3 direction)
    {
        if (direction.sqrMagnitude != 1)
            throw new ArgumentException("TryRaycastHorizontal: Direction vector not normalized");

        float numSections = _collider.bounds.size.y / GameSettings.TileSize + 1;
        Vector3 center = _collider.bounds.center;
        Vector2 raycastDir = direction * _collider.bounds.size.x / 2;

        _result.newPos = GetResultPosHorizontal(_result.newPos, numSections, center, raycastDir);
    }

    private Vector3 GetResultPosHorizontal(Vector3 resultingPosition, float numSections, Vector3 center, Vector2 raycastDir)
    {
        for (int i = 0; i <= numSections; i++)
        {
            //i==0 top side corner
            //i==1 bottom side corner
            float ySectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.y - GameSettings.CollisionOffsetValue * 2f);
            Vector2 raycastOrig = center + Vector3.up * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue) + (Vector3.down * ySectionValue);

            Vector3 collisionPosition = GetCollisionPositionHorizontal(raycastOrig, raycastDir);

            float distanceToCollisionPoint = (collisionPosition - _result.newPos).sqrMagnitude;
            float distanceToCurrentResultPosition = (resultingPosition - _result.newPos).sqrMagnitude;

            if (distanceToCollisionPoint > distanceToCurrentResultPosition)
                resultingPosition = collisionPosition;
        }

        return resultingPosition;
    }

    private Vector3 GetCollisionPositionHorizontal(Vector2 raycastOrig, Vector2 raycastDir)
    {
        RaycastHit2D hitResult = Physics2D.Raycast(raycastOrig, raycastDir, raycastDir.magnitude, _collisionMask);
        var hitSomething = hitResult.collider != null;
        _logger.DebugLine(raycastOrig, raycastOrig + raycastDir, hitSomething ? Color.red : Color.green);

        if (!hitSomething)
        {
            return _result.newPos;
        }

        if (raycastDir.x > 0)
            _result.hitRight = true;
        else
            _result.hitLeft = true;
        return new Vector3(hitResult.point.x + (-_collider.offset.x), _result.newPos.y) - new Vector3(raycastDir.x, raycastDir.y);
    }

    private void TryMoveHorizontal(Vector3 amount)
    {
        Vector3 newPos = _result.newPos + new Vector3(amount.x, 0);
        float numSections = _collider.bounds.size.y / GameSettings.TileSize + 1;
        bool movingRight = amount.x > 0;
        Vector3 moveDirection = movingRight ? Vector3.right : Vector3.left;
        Vector3 resultingPosition = _result.newPos + new Vector3(amount.x, 0);

        for (int i = 0; i <= numSections; i++)
        {
            //i==0 top side corner
            //i==1 bottom side corner
            float ySectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.y - GameSettings.CollisionOffsetValue * 2f);
            Vector2 orig = _collider.bounds.center + Vector3.up * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue) + (Vector3.down * ySectionValue);

            resultingPosition = GetMoveCollisionPositionHorizontal(orig, moveDirection, amount, resultingPosition);

            float distToCollisionPoint = (resultingPosition - _result.newPos).sqrMagnitude;
            float distToCurrentResultPosition = (newPos - _result.newPos).sqrMagnitude;

            if ((movingRight && newPos.x > resultingPosition.x)
                || (!movingRight && newPos.x < resultingPosition.x))
            {
                newPos = resultingPosition;   //Basically we ran into something so use this temppos
            }
        }

        _result.newPos = newPos;
    }

    private Vector3 GetMoveCollisionPositionHorizontal(Vector2 orig, Vector3 direction, Vector3 moveAmount, Vector3 targetPosition)
    {
        float distance = Mathf.Abs(moveAmount.x) + GameSettings.CollisionOffsetValue + _collider.size.x / 2f;

        RaycastHit2D hitResult = Physics2D.Raycast(orig, direction, distance, _collisionMask);
        bool hitSomething = hitResult.collider != null;

        _logger.DebugLine(orig, orig + new Vector2(direction.x, direction.y).normalized * distance, hitSomething ? Color.red : Color.green);

        if (!hitSomething)
        {
            return targetPosition;
        }

        if (direction.x > 0)
            _result.hitRight = true;
        else
            _result.hitLeft = true;

        float xCollisionPos = hitResult.point.x + (- _collider.offset.x);
        return new Vector3(xCollisionPos, targetPosition.y) - direction * (_collider.bounds.size.x / 2f);
    }

    private void TryMoveVertical(Vector3 amount)
    {
        bool movingDown = amount.y < 0;
        Vector3 moveVect = movingDown ? Vector3.down : Vector3.up;
        Vector3 newPos = _result.newPos + new Vector3(0, amount.y);
        float numSections = Mathf.Max(1, _collider.bounds.size.x / GameSettings.TileSize + 1);
        Vector3 verticalCenter = GetCenterPointOfBox(moveVect);
        _logger.DebugLine(verticalCenter, verticalCenter, Color.white);

        for (int i = 0; i <= numSections; i++)
        {
            Vector3 resultingPos = _result.newPos + new Vector3(0, amount.y);
            //i==0 left vertical corner
            //i==1 right vertical corner
            float xSectionValue = Mathf.Min(i * GameSettings.TileSize, _collider.bounds.size.x - GameSettings.CollisionOffsetValue * 2f);
            Vector2 orig = verticalCenter + Vector3.left * (_collider.bounds.size.x / 2 - GameSettings.CollisionOffsetValue) + (Vector3.right * xSectionValue);
            RaycastHit2D hitResult = Physics2D.Raycast(orig, moveVect, Mathf.Abs(amount.y) + GameSettings.CollisionOffsetValue, _collisionMask);
            if (hitResult)
            {
                _logger.DebugLine(orig, orig + new Vector2(moveVect.x, moveVect.y).normalized * Mathf.Abs(amount.y), Color.red);

                if (movingDown)
                    _result.hitDown = true;
                else
                    _result.hitUp = true;
                resultingPos = new Vector3(resultingPos.x, hitResult.point.y + (-_collider.offset.y)) - moveVect * (_collider.bounds.size.y / 2f);
            }
            else
            {
                _logger.DebugLine(orig, orig + new Vector2(moveVect.x, moveVect.y).normalized * Mathf.Abs(amount.y), Color.green);
            }

            if ((movingDown && newPos.y < resultingPos.y)
                || (!movingDown && newPos.y > resultingPos.y))
                newPos = resultingPos;   //Basically we ran into something so use this temppos
        }

        _result.newPos = newPos;
    }

    private Vector3 GetCenterPointOfBox(Vector3 side)
    {
        if (side != Vector3.left && side != Vector3.right && side != Vector3.up && side != Vector3.down)
            throw new ArgumentException("Unexpected side: " + side.ToString());

        if (side.x != 0)
            return _result.newPos + _collider.offset.ToVector3() + side * (_collider.bounds.size.x / 2 - GameSettings.CollisionOffsetValue);
        else
            return _result.newPos + _collider.offset.ToVector3() + side * (_collider.bounds.size.y / 2 - GameSettings.CollisionOffsetValue);
    }
}
