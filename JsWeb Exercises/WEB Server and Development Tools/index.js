const http = require('http');
const port = 8080;
const url = require('url');
const handlers = require('./handlers/index');
http
    .createServer((req, res) => {
        req.path = url.parse(req.url).pathname;

        for(let handler of handlers) {
            if(!handler(req, res)) {
                break;
            }
        }
    })
    .listen(port);

console.log(`Server listening on port ${port}`);