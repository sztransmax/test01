# test01
Write .NET console app that:  
* Takes as a parameter a string that represents a text file containing a list of names, and their
scores

* rders the names by their score. If scores are the same, order by their last name followed by
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
