const formidable = require('formidable');
const Image = require('mongoose').model('Image');

module.exports = (req, res) => {
  if (req.pathname === '/addImage' && req.method === 'POST') {
    addImage(req, res)
  } else if (req.pathname === '/delete' && req.method === 'GET') {
    deleteImg(req, res)
  } else {
    return true
  }
}

function addImage(req, res){
  const form = new formidable.IncomingForm();

    form.parse(req, (err,fields,files) =>{
      if(err){
        throw err;
      }


      const title = fields.imageTitle;
      const description = fields.description;
      const url = fields.imageUrl;


      let tags = fields.tagsID.split(',').reduce((p,c,i,a) =>{
        if(p.includes(c) || c.length === 0){
          return p;
        }else{
          p.push(c);
          return p;
        }
      }, []);

      Image.create({
        url,
        title,
        description,
        tags
      })
      .then(image => {
        res.writeHead(302,{
          location: '/'
        })
        res.end();
      })
      .catch(err => {
        console.log(err);
        res.writeHead(500, {
          'content-type':'text/plain'
        })
        res.write('Server Error!');
        res.end();
      })
      })
  }
