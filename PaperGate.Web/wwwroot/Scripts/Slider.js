const slides = document.querySelector('.slider-items').children;
const next = document.querySelector('.next');
const prev = document.querySelector('.prev');
const totalSlide = slides.length;
console.log(slides)
let index = 0;
let duration = 6000;

next.onclick = ()=>{
  slide('next');
}

prev.onclick = ()=>{
  slide('prev');
}


function slide(direction){
  progress()
  if(direction=='next'){
    if(index == totalSlide-1){
      index = 0;
    }else{
      index ++;
    }
  }

  if(direction=='prev'){
    if(index == 0){
      index= totalSlide-1
    }else{
      index --;
    }
  }


  clearInterval(timer);
  timer = setInterval(autoSlide, duration)

  for(let i =0; i<slides.length; i++){
    slides[i].classList.remove('active')
  }

  slides[index].classList.add('active')

}



function progress(){
  document.querySelector('.progress').innerHTML = '';
  const div = document.createElement('div');
  div.style.height = '5px';
  div.style.width = '0';
  div.style.position = 'absolute';
  div.style.top = '0';
  div.style.left = '0';
    div.style.backgroundColor = '#4f94f8';
  div.id = 'progress';
  div.style.animation = 'progress ' + duration/1000 + "s linear";
  document.querySelector('.progress').appendChild(div)
}



function autoSlide(){
  slide('next');
}

let timer = setInterval(autoSlide, duration)

progress();
