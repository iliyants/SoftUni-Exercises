const fs = require('fs');
let storage = {};

module.exports={
  put:(key, value) => {
      throwErrorIfKeyIsNotString(key);
        if (!keyExistsInTheStorage){
            throw new Error(`Parameter with key value - ${key}, already exists in the storage !`);
        }
      storage[key] = value;
  },
    get:(key) => {
    throwErrorIfKeyIsNotString(key);
    if (!keyExistsInTheStorage(key)){
        throw new Error(`Parameter with key value - ${key}, does not exist in the storage !`);
    }
     return storage[key];
    },
    getAll:() => {
      if (Object.keys(storage).length === 0){
          return 'Storage was empty!';
      }
      return storage;
    },
    update:(key, newValue) =>{
      throwErrorIfKeyIsNotString(key);
      if(!keyExistsInTheStorage(key)){
          throw new Error(`Parameter with key value - ${key}, does not exist in the storage !`);
      }
      storage[key] = newValue;
    },
    delete:(key) =>{
        throwErrorIfKeyIsNotString(key);
        if(!keyExistsInTheStorage(key)){
            throw new Error(`Parameter with key value - ${key}, does not exist in the storage !`);
        }
        delete storage[key];
    },
    clear:() =>{
      storage = {};
    },
    save:() =>{
      fs.writeFileSync('storage.json', JSON.stringify(storage), 'utf-8');
    },
    load:() =>{
      if (fs.existsSync('storage.json')){
          let storageFile = fs.readFileSync('storage.json');
            return storage = JSON.parse(storageFile);
      }
    },
};


function throwErrorIfKeyIsNotString(key){
    if(typeof(key) !== "string"){
        throw new Error(`${key} parameter should be of type string !`);
    }
}
function keyExistsInTheStorage(key){
    return storage.hasOwnProperty(key);
}
