var Numeric = function () { };

// Get a random integer between 'min' and 'max'.
Numeric.getRandomInt = function(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}