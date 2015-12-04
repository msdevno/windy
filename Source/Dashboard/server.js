var express = require("express");
var app = express();
var port = process.env.PORT || 4000;
var root = typeof process.env.PORT !== "undefined" ? "./" : "public"; 

app.use(express.static("./"));
app.listen(port);