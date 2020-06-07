const fs = require('fs');
const viewHandler = require('./view-handler')
const url = require('url');

let getContentType = (url) => {
    let contentType = 'text/plain';
    if(url.endsWith('.css')) {
        contentType = 'text/css';
    } else if (url.endsWith('.js')){
        contentType = 'application/javascript';
    } else if (url.endsWith('.html')) {
        contentType = 'text/html';
    } else if (url.endsWith('.png')) {
        contentType = 'image/jpeg';
    } else if(url.endsWith('.ico')){
        contentType = 'image/x-icon';
    }

    return contentType;
}

function handleFile(req, res, data, parameter){
    let url = req.path;
    if (url.endsWith('.css')
        || url.endsWith('.js')
        || url.endsWith('.png')
        || url.endsWith('.ico')) {
        res.writeHead(200, {
            'Content-Type' : getContentType(url)
        });
        res.write(data);
        res.end();
    }else if(url.endsWith('.html')){
        res.writeHead(200, {
            'Content-Type' : getContentType(url)
        });
        let parsedData = '';
        if(req.method === 'GET'){
        parsedData = viewHandler.getViewData(req,res, data, parameter);
        }else if(req.method === 'POST'){
        parsedData = viewHandler.postViewData(req, res, data);
        }
    } 
    else{
        res.writeHead(403);
        res.write('403 Forbidden!');
        res.end();
    }
}


module.exports = (req, res) => {

    let query = url.parse(req.url, true).query;

    let parameter = query.id;

    if(req.path.startsWith('/views')){
        req.path = `${req.path}.html`;    
    }

    if((req.path.startsWith('/public') || req.path.startsWith('/views')) && req.method === 'GET'){
        fs.readFile(`.${req.path}`, (err, data) => {
            if(err){
                console.log(err);
                return;
            }
            handleFile(req,res,data, parameter);
        })
    }else if(req.method === 'POST'){
      
        fs.readFile(`.${req.path}`, (err, data) => {
            if(err){
                console.log(err);
                return;
            }
            handleFile(req,res,data, parameter);
        })
   
    }else{
        res.writeHead(404);
        res.write('404 Not Found!');
        res.end();
    }
}