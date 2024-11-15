# MeetingsLime

### Task

As a first step, management has asked for an application that works independently of the existing system and
that can be called to get suggestions for suitable meeting times based on the following parameters:
	- participants (employee ids), one or multiple
	- desired meeting length (minutes)
	- earliest and latest requested meeting date and time
	- office hours (e.g. 08-17)
The application can be either an HTTP API or a console application.

At regular intervals all information is dumped from the existing system to a number of text files where the
freebusy.txt file contains details on when all employees are busy the next few weeks.
An excerpt from the file can look like this:
	170378154979885419149243073079764064027;Colin Gomez
	170378154979885419149243073079764064027;2/18/2014 10:30:00 AM;2/18/2014 11:00:00 AM;485D2AEB9DBE3...
	139016136604805407078985976850150049467;Minnie Callahan
	139016136604805407078985976850150049467;2/19/2014 10:30:00 AM;2/19/2014 1:00:00 PM;C165039FC08AB4...
As it seems, the file has lines in two different formats where the first one contains employee id and display
name and the second format has information on the time slots where the employee is busy and therefore not
available for meetings.
The following can be good to know:
	- In the file, all times stated can be treated as local times – no need to adjust for timezone differences
	- apparently it is quite common that people work every day of the week
	- due to the crappy state of the existing system the file may contain some irregularities, these should be ignored
	- the system only handles meetings that start every whole and half hour, e.g. 08.00, 08.30, 09.00, etc.

### Solution
1. Load the files correctly (Service for the files) -> ins constructor, file data is serving as DB tables (it should be one per application runtime - then consider singleton patern?
or I should consider it's constantly ingoing?')
2. Check if the files were loaded correctly (any empty lines does are left with the names or hours)
	Which parser are quicker Newtonsoft, System.Json?

3. Where do we store the available slots (Hashtable, Dictionary) - describe the complexity of the queries, the list of available slots is big so it may have some impact?
read how to read data from big files, maybe some algorith of searching in the collection?
4. Precise if we just consider some specific point in time - meaning that this data is not constantly //I think nope it's my edge case suspition
5. Do the set of endpoints for getting the meeting times availability for each separate staff and one big endpoint for all of the items selected
6. Treat it as the REST API, prepare the input validations (think about caching for performance?)
7. Write unit tests for business logic, write how would you behave if that will be the DB



### How to run it locally