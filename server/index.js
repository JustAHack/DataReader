const express = require('express')
const app = express()
const http = require('http')
const server = http.createServer(app)
const port = 3000

app.use(express.json( { limit: '20mb' }))

let jsonData = {}

app.get('/', (req, res) => {
  res.send(`Welcome, [get] and [post] requests are available <a href="data">here</a>`)
})

app.get('/data', (req, res) => {
  res.send(jsonData)
})

app.post('/data', (req, res) => {
  jsonData = req.body
  res.send(jsonData).status(200)
})

server.listen(port, () =>
{
  console.log(`Server running on port ${port}`)
})