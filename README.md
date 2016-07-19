# test01
Write .NET console app that:  
* Takes as a parameter a string that represents a text file containing a list of names, and their
scores

* Orders the names by their score. If scores are the same, order by their last name followed by
first name

* Creates a new text file called &lt;input-file- name&gt;-graded.txt with the list of sorted score and
names.

For example, if the input file contains:

BUNDY, TERESSA, 88  
SMITH, ALLAN, 70  
KING, MADISON, 88  
SMITH, FRANCIS, 85  

Then the output file would be:

BUNDY, TERESSA, 88  
KING, MADISON, 88  
SMITH, FRANCIS, 85  
SMITH, ALLAN, 70  


**Example of console execution**  
grade-scores c:\names.txt

BUNDY, TERESSA, 88  
KING, MADISON, 88  
SMITH, FRANCIS, 85  
SMITH, ALLAN, 70  

Finished: created names-graded.txt

# Answer
As you can see there is a project here (i.e. GradeScores) which uses class SortTextFile to read a text file and generate the sorted version of that. Actual sorting is done via
```
SortFile(string inputFile, string outputFile, long chunkSize)
```
It needs three parameters:  
**inputFile:** which is the full path of text file to be sorted  
**outputFile:** file which will included the sorted rows of inputFile  
**chunkSize:** that is the size of temporary chunk files used during the sort process  

**Note:** this is based on k-way merge sort which is optimized for very large files. The biggest advantage of this algorithem is it does not need to load whole file into memory (which is impossible when the file size is larger than available memory).
The algorithm starts with splitting the input file into sorted chunks. For handling duplicate lines, a priority queue has been used (which is based on a max heap).


