SELECT d.Id, d.Name, * FROM apotheca.Documents d
WHERE Contains(d.Document, '%regular%')