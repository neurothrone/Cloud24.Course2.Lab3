# Cloud24.Course2.lab3

### Setup

Inside the `SimpleStore.Maui.Client` project, open `appsettings.json` and replace `USERNAME` and `PASSWORD` with your
**MongoDB Atlas** username and password for the **MongoDbCloudConnection** key.

```json
{
  "ConnectionStrings": {
    "MongoDbLocalConnection": "mongodb://localhost:27017",
    "MongoDbCloudConnection": "mongodb://USERNAME:PASSWORD@cloud24-shard-00-00.w61l0.mongodb.net:27017,cloud24-shard-00-01.w61l0.mongodb.net:27017,cloud24-shard-00-02.w61l0.mongodb.net:27017/?ssl=true&replicaSet=atlas-24wh1w-shard-0&authSource=admin&retryWrites=true&w=majority"
  }
}
```