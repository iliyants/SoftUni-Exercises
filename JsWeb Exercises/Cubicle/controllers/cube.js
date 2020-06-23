let Cube = require('../models/CubeSchema');

function handleErrors(error, res,body){
    let errors = [];

    for(const prop in error.errors){
        errors.push(error.errors[prop].message);
    }

    res.locals.globalErrors = errors;
    res.render('create', body);
}

module.exports = {
    createGet: (req,res) =>{
        res.render('create');
    },
    createPost: (req,res) =>{
        const body = req.body;
        body.difficulty = Number(body.difficulty);

        Cube.create(body)
        .then((c) =>{
            res.redirect('/');
        })
        .catch((e) => {
            handleErrors(e, res, body);
        });
    },
    details: (req,res) =>{

        let id = req.params.id;
        
        Cube
        .findById(id)
        .then((obj) => {
            let cube = JSON.parse(JSON.stringify(obj));
            res.render('details', cube);
        })
        .catch((e) => {
            console.log(e);
        })
    }
}