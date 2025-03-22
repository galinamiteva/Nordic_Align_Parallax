


var rellax = new Rellax('.parallax');


const sr = ScrollReveal({
    duration: 2500,
    reset: true
});

/*Data*/
sr.reveal('.section__data', { origin: 'left', distance: '70px' });

/*Imgs*/
sr.reveal('.section__img', { origin: 'left', distance: '90px', delay: 200 }); 
