const db = require('../config/dataBase');
const qs = require('querystring');

const getViewData = (req,res, data, parameter) => {

    if (req.path.endsWith('/viewAll.html')) {
        let allMoviesHTML = '';

        for (const movie of db) {
            let singleMovieHTML = '<div class="movie">';
            singleMovieHTML += `<a href=/views/details?id=${movie.id}><img class="moviePoster" src="${movie.moviePoster}"></a>`;
            singleMovieHTML += '</div>';
            allMoviesHTML += singleMovieHTML;
        }

       data =  data.toString().replace('content', allMoviesHTML);
    } else if (req.path.endsWith('/details.html')) {

        var movie = db.find(x => x.id.toString() === parameter);

        let detailsHTML = `<div class="content">
        <img src="${movie.moviePoster}" alt="" />
        <h3>Title ${movie.movieTitle}</h3>
        <h3>Year ${movie.movieYear}</h3>
        <p> ${movie.movieDescription}</p>
        </div>`;

        data = data.toString().replace('content', detailsHTML);
    }

    res.write(data);
    res.end();
}

const postViewData = (req,res, pageData) => {

    let body = '';
    let formData = '';
    let responseHTML = '';

    req.on('data', function (chunk) {
        body += chunk;
    });

    req.on('end', () => {
        formData = qs.parse(body);

        if (req.path.endsWith('/addMovie.html')) {
    
            let formDataLength = Object.values(formData).filter(x => x !== "").length;
    
            if (formDataLength === 4) {
                db.push({ 
                    "id": db.length + 1,
                    "movieTitle": formData.movieTitle,
                    "movieYear": formData.movieYear,
                    "moviePoster": formData.moviePoster,
                    "movieDescription": formData.movieDescription,
                  });
    
                responseHTML = `<div id="succssesBox">
                      <h2 id="succssesMsg">Movie Added</h2> </div>`;
            } else {
                responseHTML = `<div id="errBox">
                         <h2 id="errMsg">Please fill all fields</h2> </div>`;
            }

            pageData = pageData.toString().replace('content', responseHTML);

            res.write(pageData);
            res.end();
        }
    });
}


exports.getViewData = getViewData;
exports.postViewData = postViewData;
