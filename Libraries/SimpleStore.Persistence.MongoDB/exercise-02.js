// 1. Använd en databas med namn myDB.
use MyDB

db.authors.find()

// 2. Skapa ett dokument med innehåll FirstName: ”Selma”, LastName: Lagerlöf
// och sätt in det i en kollektion med namn ”authors”.
db.authors.insertOne({first_name: "Selma", last_name: "Lagerlöf"})

// 3. Lägg till ytterligare ett dokument i ”authors” med
// FirstName: ”August”, LastName: ”Bondeson”, Birth: 1854
db.authors.insertMany(
    [
        {first_name: "Selma", last_name: "Lagerlöf"},
        {first_name: "August", last_name: "Bondeson", birth_year: 1854},
        {first_name: "Hjalmar", last_name: "Unknown"}
    ]
)

// 4. Uppdatera dokumentet för August Bondeson och lägg till Death: 1906
db.authors.updateOne({first_name: "August"}, {$set: {death_year: 1906}})

// 5. Lägg till ytterligare författare i ”authors” genom att köra kommandot
// load("addAuthors.js") från mongo Shell. [addAuthors.js](./addAuthors.js).
// Eller öppna den filen och copy/pasta innehållet i mongosh.

// 6. Räkna hur många dokument det finns totalt i ”authors”.
db.authors.countDocuments()
db.authors.countDocuments({birth_year: {$exists: true}})

db.authors.aggregate([
    {
        $count: "authors_count"
    },
])

db.authors.aggregate([
    {
        $match: {birth_year: {$exists: true}}
    },
    {
        $count: "authors_count"
    },
])

db.authors.aggregate([
    {
        $match: {birth_year: 1854}
    },
    {
        $count: "authors_count"
    },
])

// 7. Räkna hur många författare som heter August i förnamn.
db.authors.aggregate([
    {
        $match: {first_name: "August"}
    },
    {
        $count: "authors_count"
    },
])

// 8. Lägg till Birth: 1858 och Death: 1940 för Selma Lagerlöf
db.authors.updateOne(
    {first_name: "Selma", last_name: "Lagerlöf"},
    {$set: {birth_year: 1858, death_year: 1940}}
)

// 9. Lägg till en array ”Books” för Selma Lagerlöf med följande böcker:
//     ”Gösta Berlings saga”, ”En herrgårdssägen”, ”Nils Holgerssons underbara resa genom Sverige”
db.authors.updateOne(
    {first_name: "Selma", last_name: "Lagerlöf"},
    {
        $set: {
            books: [
                "Gösta Berlings saga",
                "En herrgårdssägen",
                "Nils Holgerssons underbara resa genom Sverige"
            ]
        }
    }
)

// 10. Lägg till boken ”Vi på Saltkråkan” bland Astrid Lindgrens böcker.
db.authors.updateOne(
    {first_name: "Astrid", last_name: "Lindgrens"},
    {
        $set: {
            books: [
                "Vi på Saltkråkan",
                "Bröderna Lejonhjärta"
            ]
        }
    },
    {upsert: true}
)

// 11. Ta bort boken ”Bröderna Lejonhjärta” från Astrid Lindgrens böcker.
db.authors.updateOne(
    {first_name: "Astrid", last_name: "Lindgrens"},
    {
        $pull: {books: {$in: ["Bröderna Lejonhjärta"]}}
    }
)

// 12. Visa dokument för de författare som dog år 2000 eller senare.
db.authors.find({death_year: {$gte: 2000}})

// 13. Räkna hur många författare som dog på 1940-talet.
db.authors.find({death_year: {$eq: 1940}}).count()

// 14. Visa endast attributen FirstName, LastName och Death för de författare som dog på 1940-talet.
db.authors.find({}, {first_name: 1, last_name: 1, death_year: 1})

// 15. Lägg till Gender: ”Male” i dokument med FirstName = ”Hjalmar”.
db.authors.updateMany({first_name: "Hjalmar"}, {$set: {gender: "Male"}})

// 16. Lägg till Gender: ”Female” i dokument där FirstName är Astrid, Karin eller Selma.
db.authors.updateMany({first_name: {$in: ["Astrid", "Karin", "Selma"]}}, {$set: {gender: "Female"}})

// 17. Ta bort arrayen Books från dokumentet med August Strindberg.
db.authors.updateOne(
    {first_name: "August", last_name: "Strindberg"},
    {
        $set: {
            books: [
                "Vi på Saltkråkan",
                "Bröderna Lejonhjärta"
            ]
        }
    },
    {upsert: true}
)
db.authors.updateOne({first_name: "August", last_name: "Strindberg"}, {$unset: {books: ""}})
// 18. Ta bort dokumenten där FirstName = ”August”.
db.authors.deleteMany({first_name: "August"})


db.authors.find()
db.authors.find().limit(2)
db.authors.find({first_name: "August"})
db.authors.find({first_name: "August", last_name: "Strindberg"})

db.authors.deleteOne({first_name: "August", last_name: "Bondeson", death_year: {$exists: false}})