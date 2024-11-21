use TestDB

// !: Create
db.users.insertOne({name: "John", age: 25, city: "Gothenburg"})
db.users.insertMany(
    [
        {name: "John", age: 25, city: "Gothenburg"},
        {name: "Jane", age: 22, city: "Stockholm"}
    ]
)

// !: Read
db.users.find()
db.users.find({name: "John"})

// !: Update
db.users.updateOne({name: "John"}, {$set: {city: "Stockholm"}})
db.users.updateMany({name: "John"}, {$set: {city: "Stockholm"}})

// !: Delete
db.users.deleteOne({name: "John"})
db.users.deleteMany({name: "John"})

// !: Quit
quit()

