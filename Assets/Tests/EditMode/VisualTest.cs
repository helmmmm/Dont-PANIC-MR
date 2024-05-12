using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.Rendering;
using System.Linq;

public class VisualTest
{
    [UnityTest]
    public IEnumerator VerifyInitialSceneRender()
    {
        // Arrange
        //yield return new WaitForSeconds(5f);
        yield return new WaitForEndOfFrame();

        Texture2D capturedScreenshot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D baselineImage = Resources.Load<Texture2D>("BaselineScreenshot");
        
        // Act
        bool isEqual = CompareTextures(capturedScreenshot, baselineImage);

        // Assert
        Assert.IsTrue(isEqual, "The screenshots do not match.");

        // Teardown
        Object.Destroy(capturedScreenshot);
    }

    private bool CompareTextures(Texture2D texA, Texture2D texB)
    {
        byte[] aBytes = texA.EncodeToPNG();
        byte[] bBytes = texB.EncodeToPNG();
        return aBytes.SequenceEqual(bBytes);
    }
}
