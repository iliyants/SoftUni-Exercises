const fs = require('fs');
const db = require('../config/dataBase');
const url = require('url');
const Busboy = require('busboy');
const shortId = require('shortid');


module.exports = (req, res) => {
  if (req.pathname === '/viewAllMemes' && req.method === 'GET') {
    viewAll(req, res);
  } else if (req.pathname === '/addMeme' && req.method === 'GET') {
    viewAddMeme(req, res);
  } else if (req.pathname === '/addMeme' && req.method === 'POST') {
    addMeme(req, res);
  } else if (req.pathname.startsWith('/getDetails') && req.method === 'GET') {
    getDetails(req, res);
  } else if (req.pathname.startsWith('public/memeStorage') && req.method === 'GET') {
    console.log('HERE');
  }
  else {
    return true
  }
}


function viewAddMeme(req, res) {
  fs.readFile(`./views${req.pathname}.html`, (err, data) => {
    if (err) {
      console.log(err);
      return;
    }
    res.writeHead(200, {
      'Content-Type': 'text/html'
    });
    res.end(data);
  })
}

function addMeme(req, res) {

  let id = shortId.generate();
  let source = `./public/memeStorage/${id}.jpg`;
  let busboy = new Busboy({ headers: req.headers });
  let meme = {
    id: id,
    memeSrc: source,
    dateStamp: Date.now()
  };


  busboy.on('file', function (fieldname, file) {
    file.pipe(fs.createWriteStream(source));
  });
  busboy.on('field', function (fieldname, val) {
    meme[fieldname] = val;
  });
  busboy.on('finish', function () {
    db.add(meme);
    db.save(meme)
      .then(function () {
        res.writeHead(303, { Connection: 'close', Location: '/viewAllMemes' });
        res.end();
      })
  });
  req.pipe(busboy);
}



function getDetails(req, res) {
  fs.readFile(`./views/details.html`, (err, data) => {
    if (err) {
      console.log(err)
      return
    } else {

      const query = url.parse(req.url, true).query;

      const memeId = query.id;

      db.load()
        .then(function (memes) {

          const meme = memes.find(x => x.id.toString() === memeId);

          let resultHTML = `<div class="content">
          <img src="${meme.memeSrc}" alt=""/>
          <h3>Title ${meme.title}</h3>
          <p> ${meme.description}</p>
          </div>`;

          return data = data.toString()
            .replace('<div id="replaceMe">{{replaceMe}}</div>', resultHTML);

        }).then(function (data) {
          res.writeHead(200, {
            'Content-Type': 'text/html'
          });
          res.end(data);
        });
    }
  });
}

function viewAll(req, res) {
  fs.readFile(`./views${req.pathname}.html`, (err, data) => {
    if (err) {
      console.log(err)
      return
    } else {

      db.load()
        .then(function (memes) {

          let resultHTML = '';
          
          memes.sort((a, b) => (a.dateStamp > b.dateStamp) ? 1 : -1);

          memes.forEach((meme) => {
            if (meme.privacy == 'on') {
              resultHTML += `<div class="column">
              <a href="/getDetails?id=${meme.id}">
            <img src=${meme.memeSrc} alt="Snow" style="width:100%">
              </div>`;
            }

          });

          return data = data.toString()
            .replace('<div id="replaceMe">{{replaceMe}}</div>', resultHTML);

        }).then(function (data) {
          res.writeHead(200, {
            'Content-Type': 'text/html'
          });
          res.end(data);
        });
    }
  });
}

