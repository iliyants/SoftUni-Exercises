const mongoose = require('mongoose');

function imageUrlValidator(imageUrl){
    return (imageUrl.endsWith('.jpg') || imageUrl.endsWith('.png'));
}

const cubeSchema = new mongoose.Schema({
    name:{
        type:String,
        minlength:[3, 'Name cant have less than 3 characters.'],
        maxlength:[15, 'Name cant have more than 15 characters.'],
        required:[true, 'Name is required.']
    },
    description:{
        type:String,
        minlength:[20, 'Description cant have less than 20 characters.'],
        maxlength:[300, 'Description cant be more that 300 characters.'],
        required: [true, 'Description is required.']
    },
    imageUrl:{
        type:String,
        validate:{validator:imageUrlValidator, msg:'Url must start with https:// and end on either .jpg or .png.'},
        required: [true, 'Image URL is required.']
    },
    difficulty:{
        type:Number,
        min:[1, 'Difficulty lvl must be between 1 and 6.'],
        max:6,
        required:[true, 'Difficulty lvl is required.']
    }
});

module.exports=mongoose.model('Cube', cubeSchema);