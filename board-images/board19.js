var canvas = document.getElementById('myCanvas');
var ctx = canvas.getContext('2d');

// var background = new Image();
// background.src = "/Users/dannysouthard/Downloads/board-background.jpeg";

// background.onload = function(){
//     ctx.drawImage(background, 0, 0);  
    
//     for (var i = 0; i < 19; i++) {
//         ctx.beginPath();
//         ctx.moveTo(30 + (i * 30), 30);
//         ctx.lineTo(30 + (i * 30), 570);
//         ctx.moveTo(30, 30 + (i * 30));
//         ctx.lineTo(570, 30 + (i * 30));
//         ctx.closePath();
//         ctx.stroke();
//     }

//     ctx.beginPath();
//     ctx.moveTo()
// }

ctx.beginPath();
ctx.rect(0, 0, 620, 620);
ctx.closePath();
ctx.fillStyle = '#db7';
ctx.fill();

for (var i = 0; i < 19; i++) {
    ctx.beginPath();
    ctx.moveTo(40 + (i * 30), 40);
    ctx.lineTo(40 + (i * 30), 580);
    ctx.moveTo(40, 40 + (i * 30));
    ctx.lineTo(580, 40 + (i * 30));
    ctx.closePath();
    ctx.stroke();
}

for (var j = 0; j < 3; j++) {
    for (var k = 0; k < 3; k++) {
        ctx.beginPath();
        var xlocation = j * 180 + 130;
        var ylocation = k * 180 + 130;
        ctx.arc(xlocation, ylocation, 3, 0, 2 * Math.PI);
        ctx.fillStyle = 'black';
        ctx.fill();
    }
}

ctx.font = "10px Verdana"
for (var x = 1; x < 20; x++) {
    if (x < 10) {
        ctx.fillText("" + x, 15, 585 - ((x - 1) * 30));
        ctx.fillText("" + x, 605, 585 - ((x - 1) * 30));
    } else {
        ctx.fillText("" + x, 10, 585 - ((x - 1) * 30));
        ctx.fillText("" + x, 600, 585 - ((x - 1) * 30));
    }
}

for (var y = 0; y < 19; y++) {
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 20);
    ctx.fillText(String.fromCharCode(65 + y), 35 + (y * 30), 610);
}


