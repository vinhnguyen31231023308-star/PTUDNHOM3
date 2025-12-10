const images = [
    "img/imgdetail/sp1.1.jpg",
    "img/imgdetail/sp1.3.jpg",
    "img/imgdetail/sp1.4.jpg",
    "img/imgdetail/sp1.2.jpg"
];
let currentIndex = 0;
const mainImg = document.getElementById('mainImg');
const thumbItems = document.querySelectorAll('.thumb-item');

function updateGallery() {
    mainImg.style.opacity = 0;
    setTimeout(() => {
        mainImg.src = images[currentIndex];
        mainImg.style.opacity = 1;
    }, 200);
    thumbItems.forEach((el, index) => {
        if (index === currentIndex) el.classList.add('active');
        else el.classList.remove('active');
    });
}

thumbItems.forEach((item, index) => {
    item.addEventListener('click', function () {
        currentIndex = index;
        updateGallery();
    });
});

document.querySelector('.gallery-nav-btn.prev').addEventListener('click', function () {
    currentIndex = (currentIndex > 0) ? currentIndex - 1 : images.length - 1;
    updateGallery();
});

document.querySelector('.gallery-nav-btn.next').addEventListener('click', function () {
    currentIndex = (currentIndex < images.length - 1) ? currentIndex + 1 : 0;
    updateGallery();
});

function updateQty(change) {
    const input = document.getElementById('qtyInput');
    let val = parseInt(input.value) + change;
    if (val < 1) val = 1;
    input.value = val;
}

document.querySelectorAll('.size-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        document.querySelectorAll('.size-btn').forEach(el => el.classList.remove('active'));
        this.classList.add('active');
    });
});

function openTab(evt, tabName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tab-content");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
        tabcontent[i].classList.remove("active");
    }
    tablinks = document.getElementsByClassName("nav-link");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).style.display = "block";
    document.getElementById(tabName).classList.add("active");
    evt.currentTarget.className += " active";
}
// --- RELATED PRODUCTS SLIDER LOGIC ---
function scrollRelated(direction) {
    const track = document.getElementById('relatedTrack');
    const cardWidth = 300; // Chiều rộng thẻ + gap ước lượng
    const scrollAmount = cardWidth * 1; // Mỗi lần bấm trượt 1 thẻ

    if (direction === 1) {
        track.scrollBy({ left: scrollAmount, behavior: 'smooth' });
    } else {
        track.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
    }
}
if (history.scrollRestoration) {
    history.scrollRestoration = 'manual';
} else {
    window.onbeforeunload = function () {
        window.scrollTo(0, 0);
    }
}
