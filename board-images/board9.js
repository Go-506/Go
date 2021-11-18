// a 9x9 board
var canvas = document.getElementById('myCanvas');
var ctx = canvas.getContext('2d');

ctx.beginPath();
ctx.rect(0, 0, 320, 320);
ctx.closePath();
ctx.fillStyle = '#db7';
ctx.fill();

for (var i = 0; i < 9; i++) {
    ctx.beginPath();
    ctx.moveTo(40 + (i * 30), 40);
    ctx.lineTo(40 + (i * 30), 280);
    ctx.moveTo(40, 40 + (i * 30));
    ctx.lineTo(280, 40 + (i * 30));
    ctx.closePath();
    ctx.stroke();
}

for (var j = 0; j < 2; j++) {
    for (var k = 0; k < 2; k++) {
        ctx.beginPath();
        var xlocation = j * 120 + 100;
        var ylocation = k * 120 + 100;
        ctx.arc(xlocation, ylocation, 3, 0, 2 * Math.PI);
        ctx.fillStyle = 'black';
        ctx.fill();
    }
}
ctx.beginPath();
ctx.arc(160, 160, 3, 0, 2 * Math.PI);
ctx.fill();

ctx.font = "10px Verdana"
for (var x = 1; x < 10; x++) {
    if (x < 10) {
        ctx.fillText("" + x, 15, 285 - ((x - 1) * 30));
        ctx.fillText("" + x, 305, 285 - ((x - 1) * 30));
    } else {
        ctx.fillText("" + x, 10, 285 - ((x - 1) * 30));
        ctx.fillText("" + x, 300, 285 - ((x - 1) * 30));
    }
}

for (var y = 0; y < 9; y++) {
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 20);
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 310);
}
