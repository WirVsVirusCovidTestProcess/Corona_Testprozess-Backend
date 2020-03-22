# Corona Testprozess Backend

> Contains all the backend stuff for the testprozess tool.

## Use the api

### 1 Add the answered questions

- URL: `/api/SaveQuestionData`
- Method: `POST`
- Body: 

```json
{
  "Answers": [
    {
      "A": "1"
    },
    {
      "B": "2"
    },
    {
      "C": "1"
    }
  ]
}
```

- Result Body contains question token -> smaple: rJcY4EDO10hAVGlmMFsgTKmULsW0EuQC

### 1.1 (Optional) get back the question data

This is to check if the risk level is calculated and to get back the posted data.

- URL: `/api/GetDataFromToken`
- Method: POST
- Body:

```json
{
    "Token": "rJcY4EDO10hAVGlmMFsgTKmULsW0EuQC"
}
```

- Result Body contains question token -> sample: 

```json
{
    "id":"5757effc-30a9-4703-84cd-e9ac003cbb21",
    "source":"covapp.charite",
    "token":"rJcY4EDO10hAVGlmMFsgTKmULsW0EuQC",
    "answers":[
        {"A":"1"},
        {"B":"2"},
        {"C":"1"}],
    "riskScore":1
}
```

## Add Userinforamtion

Add personal information of the user to the backend.

- URL `/api/AddUserInformation`
- Method: POST
- Body:

```json
{
    "FirstName": "Paul", 
    "LastName": "Jeschke", 
    "Email": "paul-jeschke@outlook.com", 
    "Location": "00000"
}
```

- Result Body contains usertoken -> sample: V4AwB0HO10j0o5ZtI+JlS7UgdB1xc5M5

## Add Risk score to user infomarion

- URL `/api/UpdateRiskLevelOfTheUser`
- Method: POST
- Body:

```json
{
    "UserToken": "V4AwB0HO10j0o5ZtI+JlS7UgdB1xc5M5", 
    "QuestionToken": "rJcY4EDO10hAVGlmMFsgTKmULsW0EuQC"
}
```

- Result Body contains all user information -> sample:

```json
{
    "questionToken":null,
    "riskScore":1,
    "id":"02db9c7d-5cb4-497b-a383-4a8ebd5f0409",
    "token":"V4AwB0HO10j0o5ZtI+JlS7UgdB1xc5M5",
    "source":"covapp.charite",
    "name":null,
    "email":"paul-jeschke@outlook.com",
    "location":"4",
    "appointmentToken": null
}
```

This automaticly trigger the creation of an empty appointment

## Get empty Appointment

- URL: `/api/GetAllNotAssigendAppointMents`
- Method: GET
- Query (optional): `location=postalcode`
- Result Body: 

```json
[
    {
        "id":"c3e8da56-aefc-4f8d-bebd-9cafb3aa86e8",
        "token":"xISdXkHO10iwkQQyAocRqp51prZ3MN",
        "assigend":false,
        "dateToBeInTestcenter":"0001-01-01T01:00:00+01:00",
        "testcenterAddress":null,
        "riskScore":1,
        "location":"4",
        "trackingId":null,
        "testResult":null
    }
]
```

## Set appointment

- URL: `/api/AddAnAppointmentDate`
- Method: POST
- Body: 

```json
{
    "id":"c3e8da56-aefc-4f8d-bebd-9cafb3aa86e8",
    "token":"xISdXkHO10iwkQQyAocRqp51prZ3MN",
    "assigend":false,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress": "00000",
    "riskScore":1,
    "location":"4",
    "trackingId":null,
    "testResult":null
}
```

- Result Body -> sample: 

```json
{
    "id":"9fbeaf47-4f6e-475f-8265-f7f5ae602d08",
    "token":"Wqk43kzO10jbupJPF8+DQ6BNk1g0Hj5K",
    "assigend":true,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress":"00000",
    "riskScore":1,
    "location":"4",
    "trackingId":null,
    "testResult":null
}
```

This will inform the user about the date and location

## Add tracking Id

- URL: `/api/AddTrackingId`
- MEHTOD: POST
- BODY: 

```json
{
    "id":"9fbeaf47-4f6e-475f-8265-f7f5ae602d08",
    "token":"Wqk43kzO10jbupJPF8+DQ6BNk1g0Hj5K",
    "assigend":true,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress":"00000",
    "riskScore":1,
    "location":"4",
    "trackingId":"fgkjslgjfjlk",
    "testResult":null
}
```

- Result Body -> sample:

```json
{
    "id":"9fbeaf47-4f6e-475f-8265-f7f5ae602d08",
    "token":"Wqk43kzO10jbupJPF8+DQ6BNk1g0Hj5K",
    "assigend":true,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress":"00000",
    "riskScore":1,
    "location":"4",
    "trackingId":"fgkjslgjfjlk",
    "testResult":null
}
```

## Add test result to appointment

- URL: `/api/AddTestResultToAppointment`
- MEHTOD: POST
- BODY: 

```json
{
    "id":"9fbeaf47-4f6e-475f-8265-f7f5ae602d08",
    "token":"Wqk43kzO10jbupJPF8+DQ6BNk1g0Hj5K",
    "assigend":true,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress":"00000",
    "riskScore":1,
    "location":"4",
    "trackingId":"fgkjslgjfjlk",
    "testResult":true
}
```

- Result Body -> sample:

```json
{
    "id":"9fbeaf47-4f6e-475f-8265-f7f5ae602d08",
    "token":"Wqk43kzO10jbupJPF8+DQ6BNk1g0Hj5K",
    "assigend":true,
    "dateToBeInTestcenter":"2020-03-27T01:00:00+01:00",
    "testcenterAddress":"00000",
    "riskScore":1,
    "location":"4",
    "trackingId":"fgkjslgjfjlk",
    "testResult":true
}
```

This will inform the user about the test result.


