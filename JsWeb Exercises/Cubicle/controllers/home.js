const Cube = require('../models/CubeSchema');

module.exports = {
    homeGet: (req, res) => {
        Cube.find({})
            .select('_id name imageUrl difficulty')
            .sort('difficulty')
            .then((objects) => {

                let cubes = [];
                for (let i = 0; i < objects.length; i++) {
                    let cube = JSON.parse(JSON.stringify(objects[i]._doc));
                    cubes.push(cube);
                }
                res.render('index', { cubes });
            })
            .catch((e) => {
                console.log(e);
            })
    },
    about: (req, res) => {
        res.render('about');
    },
    search: (req, res) => {
        let { search, from, to } = req.query;

        if (from === "" && to === "") {
            fulfillSearch(search, from, to, res);
        } else {
            let errors = checkForErrors(from, to);
            if (errors.length > 0) {
                res.locals.globalErrors = errors;
                res.render('index');
                return;
            } else {
                fulfillSearch(search, from, to, res);
            }
        }

    }
}


function checkForErrors(from, to) {
    from = Number(from);
    to = Number(to);

    let errors = [];

    if (from < 1 || to > 6 || from > 6 || to < 1 || (from > to)) {
        errors.push('Difficulty "from" and "to" cannot have values bellow 1 or above 6 and difficulty "from" cannot have a greater value than "to".');
    }

    return errors;
}

function fulfillSearch(search, from, to, res) {

    Cube
        .find({})
        .then((objects) => {

            let filtered = returnFilteredArray(objects, search, from, to);

            let cubes = [];
            for (let i = 0; i < filtered.length; i++) {
                let cube = JSON.parse(JSON.stringify(filtered[i]._doc));
                cubes.push(cube);
            }
            res.render('index', { cubes });
        })
        .catch((e) => {
            console.log(e);
        })

}

function returnFilteredArray(objects, search, from, to) {
    from = Number(from);
    to = Number(to);

    let filtered = [];

    if (search && from && to) {
        filtered = objects.filter(function(result){
            return result.name.toLowerCase().includes(search.toLowerCase())
            && result.difficulty >= from && result.difficulty <= to;
        });
    } else if(!search && from && to){
        filtered = objects
            .filter(x => x.difficulty >= from && x.difficulty <= to);
    }else{
        filtered = objects
        .filter(x => x.name.toLowerCase().includes(search.toLowerCase()))
    }

    return filtered;
}