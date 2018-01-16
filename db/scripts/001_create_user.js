db.createUser({
   user: "apotheca",
   pwd: "apotheca",
   roles: [ { role: "readWrite", db: "apotheca" } ]
})