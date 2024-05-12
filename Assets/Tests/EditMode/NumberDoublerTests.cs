using NUnit.Framework;
using UnityEngine;
using Tests;

public class NumberDoublerTests
{
    [Test]
    public void DoubleNumber_WhenSpacePressed_DoublesNumber()
    {
        // Arrange
        var gameObject = new GameObject();
        var doubler = gameObject.AddComponent<NumberDoubler>();
        doubler.number = 1;

        // Act
        doubler.DoubleNumber();

        // Assert
        Assert.AreEqual(2, doubler.number, "The number should double when space is pressed.");
    }
}
