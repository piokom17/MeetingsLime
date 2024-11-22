# MeetingsLime

### How to run it locally

#1. Run it with Docker
   To run the app with docker please navigate to the path here the project main folder is located.
   In my case it was:  C:\Users\User\DEV\MeetingsLime

   Build the project with Docker, enter in command line: docker-compose build

   After build is successful run the project. Enter in command line: docker-compose up

   Open the browse and navigate to http://localhost:8080/swagger/index.html

   You will see one endpoint HTTP GET Meetings please enter the request data and send the request to get the result


#2. Run it locally without Docker
   To run the app locally first write the full path to your file in the MeetingData.cs, press the RUN button, it will automatically open the browser and navigate to the https://localhost:7110/swagger/index.html

#3. Run Unit Tests
   To run the unit tests, go to Tests -> Run All Tests, or run them from the console with: dotnet test

### My aasumptions & decisions
	- I decided to create the REST API with one HTTP GET endpoint, where all of the request parameters are required
	- I made several request data validations, which disallows to get very early or late working hours
	- The app showing the available slots, with dedicated minutes chunks, so if input is 45 minutes it will show the 45 minutes slots, I know
	that the hint was to sow them by the full halfs, but I didn't manage to do this before Friday, November 22nd at 4 PM, so that's why I left it like this
	- I decided to split the project into the .Domain and .Infrastructure parts (although I kow there is not so much business logic which should go to Domain)
	it is just for the future hypothetic development of the project
	- I created the unit tests for one request data validator, date time helper and domain service
