# My solution on Labb 3 – API in Webbapplikationer i C#, ASP.NET

# Setup
In the Solutions Explorer click the arrow next to `Connected Services`
Right-Click `SQL Server Database`
Choose Edit (or Connect)
In the first box `Connection string name` fill in:
```console
ConnectionStrings:DefaultConnection
```
In the second box `Connection string value` fill in:
<YourConnectionStringToYourDB>
> You will have to provide your own connection string to your database above

<br>

In the `Package Manager Console` write:
```console
add-migration -OutputDir Data/Migrations "Initial migration"
```
then write:
```console
update-database
```

<br>

## Database Schema
![image](https://github.com/TobiasSkog/ASP.NET-Labb-3/assets/11568812/68f13010-64d5-405a-94af-81143924cd44)

<br>

# Assignments
## Get all people in the system
Postman address
```console
https://localhost:7274/people
```
GET request
Result:
```json
[
    {
        "id": 1,
        "name": "Tobias",
        "age": 31,
        "phoneNumber": "0701234567",
        "interests": [
            {
                "id": 1,
                "title": "Coding",
                "description": "Create code on a computer",
                "links": [
                    {
                        "id": 1,
                        "website": "https://github.com/TobiasSkog?tab=repositories"
                    },
                    {
                        "id": 2,
                        "website": "https://www.cozroth.com/"
                    },
                    {
                        "id": 3,
                        "website": "https://tobiasskog.github.io/"
                    },
                    {
                        "id": 4,
                        "website": "https://www.linkedin.com/in/tobiasskog/"
                    }
                ]
            },
            {
                "id": 4,
                "title": "Gaming",
                "description": "Playing games with friends",
                "links": [
                    {
                        "id": 10,
                        "website": "https://www.pathofexile.com/"
                    }
                ]
            },
            {
                "id": 9,
                "title": "Dancinc",
                "description": "Dancing in the rain",
                "links": [
                    {
                        "id": 11,
                        "website": "https://dictionary.cambridge.org/dictionary/english/dancing"
                    },
                    {
                        "id": 12,
                        "website": "https://en.wikipedia.org/wiki/Dance"
                    }
                ]
            }
        ]
    },
    {
        "id": 2,
        "name": "Reidar",
        "age": 45,
        "phoneNumber": "0701231234",
        "interests": [
            {
                "id": 2,
                "title": "Gardening",
                "description": "Tend to flowers in a garden",
                "links": [
                    {
                        "id": 5,
                        "website": "https://en.wikipedia.org/wiki/Gardening"
                    }
                ]
            }
        ]
    }
]
```

## Get all interests that's connected to a specific person
Postman address
```console
https://localhost:7274/people/{personId}
```
GET request
Result:
```json
[
    {
        "id": 1,
        "title": "Coding",
        "description": "Create code on a computer",
        "links": [
            {
                "id": 1,
                "website": "https://github.com/TobiasSkog?tab=repositories"
            },
            {
                "id": 2,
                "website": "https://www.cozroth.com/"
            },
            {
                "id": 3,
                "website": "https://tobiasskog.github.io/"
            },
            {
                "id": 4,
                "website": "https://www.linkedin.com/in/tobiasskog/"
            }
        ]
    },
    {
        "id": 4,
        "title": "Gaming",
        "description": "Playing games with friends",
        "links": [
            {
                "id": 10,
                "website": "https://www.pathofexile.com/"
            }
        ]
    },
    {
        "id": 9,
        "title": "Dancinc",
        "description": "Dancing in the rain",
        "links": [
            {
                "id": 11,
                "website": "https://dictionary.cambridge.org/dictionary/english/dancing"
            },
            {
                "id": 12,
                "website": "https://en.wikipedia.org/wiki/Dance"
            }
        ]
    }
]
```


## Get all links that's connected to a specific person
Postman address
```console
https://localhost:7274/people/{personId}/links
```
GET request
Result:
```json
[
    {
        "id": 1,
        "website": "https://github.com/TobiasSkog?tab=repositories"
    },
    {
        "id": 2,
        "website": "https://www.cozroth.com/"
    },
    {
        "id": 3,
        "website": "https://tobiasskog.github.io/"
    },
    {
        "id": 4,
        "website": "https://www.linkedin.com/in/tobiasskog/"
    },
    {
        "id": 10,
        "website": "https://www.pathofexile.com/"
    },
    {
        "id": 11,
        "website": "https://dictionary.cambridge.org/dictionary/english/dancing"
    },
    {
        "id": 12,
        "website": "https://en.wikipedia.org/wiki/Dance"
    }
]
```

## Link a person to a new interest
Postman address
```console
https://localhost:7274/people/{personId}/interests
```
POST request
Body:
```json
{
        "Title": "Fishing",
        "Description": "Spending time outside to fish"
}
```

## Adding SINGULAR Link that's connected to a specific person and a specific interest
Postman address
```console
https://localhost:7274/people/{personId}/interests/{interestId}/link
```
POST request
Body:
```json
{
        "Website": "https://en.wikipedia.org/wiki/Fishing"
}
```


### Adding MULTIPLE Links that's connected to a specific person and a specific interest
Postman address:
```console
https://localhost:7274/people/{personId}/interests/{interestId}/link
```
POST request
Body:
```json
[
    {
        "Website": "http://www.swedenfishing.com/"
    },
    {
        "Website":"https://www.takemefishing.org/how-to-fish/how-to-catch-fish/"
    }
]
```
