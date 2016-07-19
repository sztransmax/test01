using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GradeScores.Tests
{
    [TestClass()]
    public class SortTextFileTests
    {
        private const long chunkSize = 1024 * 1024 * 1024;

        [TestMethod()]
        [DeploymentItem("Empty.txt")]
        public void SortFileTestEmptyFile()
        {
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("Empty.txt", "EmptyFile-Graded.csv", chunkSize), false);
        }

        [TestMethod()]
        [DeploymentItem("Small.txt")]
        public void SortFileTestSmallFile()
        {
            var outputFileName = Path.GetTempFileName();
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("Small.txt", outputFileName, chunkSize), true);
            File.Delete(outputFileName);
        }

        [TestMethod()]
        [DeploymentItem("Large.txt")]
        public void SortFileTestLargeFile()
        {
            var outputFileName = Path.GetTempFileName();
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("Large.txt", outputFileName, chunkSize), true);
            File.Delete(outputFileName);
        }

        [TestMethod()]
        [DeploymentItem("Small.txt")]
        public void SortFileTestCheckResultFileRows()
        {
            var outputFileName = Path.GetTempFileName();
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("Small.txt", outputFileName, chunkSize), true);
            List<string> allLinesText = File.ReadAllLines(outputFileName).ToList();
            Assert.AreEqual(allLinesText[0] , "BUNDY, TERESSA, 88");
            Assert.AreEqual(allLinesText[1], "KING, MADISON, 88");
            Assert.AreEqual(allLinesText[2], "SMITH, FRANCIS, 85");
            Assert.AreEqual(allLinesText[3], "SMITH, ALLAN, 70");
            File.Delete(outputFileName);
        }

        [TestMethod()]
        [DeploymentItem("MissingScore.txt")]
        public void SortFileTestIgnoreLinesWithMissingScore()
        {
            var outputFileName = Path.GetTempFileName();
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("MissingScore.txt", outputFileName, chunkSize), true);
            List<string> allLinesText = File.ReadAllLines(outputFileName).ToList();
            Assert.AreEqual(allLinesText[0], "BUNDY, TERESSA, 88");
            Assert.AreEqual(allLinesText[1], "SMITH, ALLAN, 70");
            File.Delete(outputFileName);
        }

        [TestMethod()]
        [DeploymentItem("DuplicateRows.txt")]
        public void SortFileTestCheckWithDuplicateRows()
        {
            var outputFileName = Path.GetTempFileName();
            var sortTextFile = new SortTextFile();
            Assert.AreEqual(sortTextFile.SortFile("DuplicateRows.txt", outputFileName, chunkSize), true);
            List<string> allLinesText = File.ReadAllLines(outputFileName).ToList();
            Assert.AreEqual(allLinesText[0], "BUNDY, TERESSA, 88");
            Assert.AreEqual(allLinesText[1], "BUNDY, TERESSA, 88");
            File.Delete(outputFileName);
        }


    }
}