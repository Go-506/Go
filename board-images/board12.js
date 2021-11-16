// a 12 x 12 board
// a 9x9 board
var canvas = document.getElementById('myCanvas');
var ctx = canvas.getContext('2d');

ctx.beginPath();
ctx.rect(0, 0, 440, 440);
ctx.closePath();
ctx.fillStyle = '#db7';
ctx.fill();

for (var i = 0; i < 13; i++) {
    ctx.beginPath();
    ctx.moveTo(40 + (i * 30), 40);
    ctx.lineTo(40 + (i * 30), 400);
    ctx.moveTo(40, 40 + (i * 30));
    ctx.lineTo(400, 40 + (i * 30));
    ctx.closePath();
    ctx.stroke();
}

for (var j = 0; j < 2; j++) {
    for (var k = 0; k < 2; k++) {
        ctx.beginPath();
        var xlocation = j * 180 + 130;
        var ylocation = k * 180 + 130;
        ctx.arc(xlocation, ylocation, 3, 0, 2 * Math.PI);
        ctx.fillStyle = 'black';
        ctx.fill();
    }
}
ctx.beginPath();
ctx.arc(220, 220, 3, 0, 2 * Math.PI);
ctx.fill();

ctx.font = "10px Verdana"
for (var x = 1; x < 14; x++) {
    if (x < 10) {
        ctx.fillText("" + x, 15, 405 - ((x - 1) * 30));
        ctx.fillText("" + x, 425, 405 - ((x - 1) * 30));
    } else {
        ctx.fillText("" + x, 10, 405 - ((x - 1) * 30));
        ctx.fillText("" + x, 420, 405 - ((x - 1) * 30));
    }
}

for (var y = 0; y < 13; y++) {
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 20);
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 430);
}