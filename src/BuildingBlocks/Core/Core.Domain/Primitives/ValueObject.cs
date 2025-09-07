/*
 * Why define ValueObject this way:
 * --------------------------------
 * 1. Value Object in DDD:
 *    - Represents a concept defined entirely by its values (no identity).
 *    - Two value objects are equal if all their properties (atomic values) are equal.
 *    - Examples: Money (Amount + Currency), Address (Street + City + ZipCode).
 *
 * 2. GetAtomicValues():
 *    - Abstract method forces each value object to specify which fields define its equality.
 *    - Ensures a consistent way to compare value objects.
 *
 * 3. Equality Handling:
 *    - Equality operators (==, !=) are overridden for intuitive comparisons.
 *    - Equals(ValueObject):
 *        - Checks same type.
 *        - Uses SequenceEqual on atomic values → ensures structural equality.
 *    - GetHashCode():
 *        - Combines hash codes of all non-null atomic values.
 *        - Ensures value objects behave correctly in collections (HashSet, Dictionary).
 *
 * 4. Null Safety:
 *    - Null checks prevent accidental null reference issues.
 *    - Non-null atomic values are used for hash code calculation.
 *
 * 5. DDD Principle:
 *    - Unlike Entity, a ValueObject has no identity (Id).
 *    - Equality is purely based on values, not references or identity.
 *
 * Overall:
 * This base class provides a reusable, type-safe, and consistent implementation
 * for all value objects in the domain, enforcing immutability and structural equality.
 */

namespace Core.Domain.Primitives;

/// <summary>
/// Base class for value objects.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the atomic values that comprise this value object.
    /// </summary>
    /// <returns>Sequence of atomic values</returns>
    public abstract IEnumerable<object> GetAtomicValues();

    #region Equality

    public static bool operator ==(ValueObject? left, ValueObject? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject? left, ValueObject? right)
        => !(left == right);

    public bool Equals(ValueObject? other)
    {
        if (other is null || GetType() != other.GetType())
        {
            return false;
        }

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override bool Equals(object? obj)
        => obj is ValueObject other && Equals(other);

    public override int GetHashCode()
        => GetAtomicValues()
            .Where(x => x is not null)
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return (current * 23) + obj!.GetHashCode();
                }
            });

    #endregion
}
