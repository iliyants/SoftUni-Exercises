const staticFilesHandler = require('./static-files');
const homePageHandler = require('./home-page');


module.exports = [
    homePageHandler,
    staticFilesHandler, 
];