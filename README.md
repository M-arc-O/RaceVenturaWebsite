# RaceVenturaWebsite
To run localy you need to take the following steps:
1 - add "JwtSecret": "3555e854-b1bd-4bfd-b86d-acdd2ca5b6eb" to the RaceVenturaAPI user secrets. The GUID can be random.
2 - If not installed get docker and run docker run --rm --name pg-docker -e POSTGRES_PASSWORD=docker -d -p 5432:5432 postgres.
3 - Open the VS solution and after that the package Manager Console in VS with project RaceVenturaAPI and run update-database.
4 - Run the code in VS with RaceVenturaAPI as startup project.
5 - If not installed get postman and post https://localhost:44305/api/accounts/ with JSON body:
        {
            "Email":"youremail",
            "Password":"yourpassword",
            "FirstName":"yourfirstname",
            "LastName":"yourlastname"
        }
    to create an account.
6 - (Optional) Import 'RaceVentura Basic Setup.postman_collection' from the PostmanCollections into postman and run for a basic setup.
7 - Open the presentation folder in VS code.
8 - Open a terminal and run npm i.
9 - Run ng serve -o.
