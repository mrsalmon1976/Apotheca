var Collections = function () { };

// Get a random integer between 'min' and 'max'.
Collections.displayList = function (arr) {
    var html = '<ul>';
    for (var i = 0; i < arr.length; i++) {
        html += '<li>' + arr[i] + '</li>';
    }
    html += '</ul>';
    return html;
}

var Numeric = function () { };

// Get a random integer between 'min' and 'max'.
Numeric.getRandomInt = function(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}