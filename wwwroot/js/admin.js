/* CinemaTickets – Admin JS */
document.addEventListener('DOMContentLoaded', function () {

    /* ── Sidebar toggle ─────────────────────────────── */
    const sidebar = document.getElementById('sidebar');
    const toggle  = document.getElementById('sidebarToggle');

    if (toggle && sidebar) {
        toggle.addEventListener('click', function () {
            if (window.innerWidth <= 768) {
                sidebar.classList.toggle('mobile-open');
            } else {
                sidebar.classList.toggle('collapsed');
            }
        });
    }

    /* close mobile sidebar on outside click */
    document.addEventListener('click', function (e) {
        if (window.innerWidth <= 768 && sidebar &&
            sidebar.classList.contains('mobile-open') &&
            !sidebar.contains(e.target) && e.target !== toggle) {
            sidebar.classList.remove('mobile-open');
        }
    });

    /* ── Actor checkbox card highlight ─────────────── */
    document.querySelectorAll('.actor-checkbox').forEach(function (cb) {
        var card = cb.closest('.actor-check-card');
        if (card) {
            if (cb.checked) card.classList.add('selected');
            cb.addEventListener('change', function () {
                card.classList.toggle('selected', this.checked);
            });
        }
    });

    /* ── Auto-dismiss flash alerts (4.5 s) ─────────── */
    setTimeout(function () {
        document.querySelectorAll('.flash-container .alert').forEach(function (el) {
            bootstrap.Alert.getOrCreateInstance(el).close();
        });
    }, 4500);

    /* ── Show validation summary if it has errors ───── */
    document.querySelectorAll('[data-valmsg-summary]').forEach(function (el) {
        el.style.display = el.querySelectorAll('li').length ? 'block' : 'none';
    });
});

/* ── Image preview helpers (global) ────────────────── */
function previewImage(input, previewId) {
    var preview = document.getElementById(previewId);
    if (!preview || !input.files || !input.files[0]) return;
    var r = new FileReader();
    r.onload = function (e) { preview.src = e.target.result; preview.style.display = 'block'; };
    r.readAsDataURL(input.files[0]);
}

function previewMainImage(input) {
    if (!input.files || !input.files[0]) return;
    var box = document.getElementById('mainImgPreviewBox');
    if (!box) return;
    var r = new FileReader();
    r.onload = function (e) {
        box.innerHTML = '<img src="' + e.target.result + '" class="main-poster-preview" />';
    };
    r.readAsDataURL(input.files[0]);
}

function previewSubImages(input) {
    var c = document.getElementById('subImgPreviews');
    if (!c) return;
    c.innerHTML = '';
    Array.from(input.files || []).forEach(function (f) {
        var r = new FileReader();
        r.onload = function (e) {
            c.innerHTML += '<div class="col-4"><img src="' + e.target.result +
                           '" class="sub-img-preview" /></div>';
        };
        r.readAsDataURL(f);
    });
}
