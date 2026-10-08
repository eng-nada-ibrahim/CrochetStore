// Global JavaScript for Crochet Store AJAX interactions

function updateCartBadge() {
    $.get('/Cart/GetCount', function (data) {
        if (data && typeof data.count !== 'undefined') {
            const badge = $('#cartItemCount');
            badge.text(data.count);
            if (data.count > 0) {
                badge.removeClass('d-none');
            } else {
                badge.addClass('d-none');
            }
        }
    });
}

function addToCart(productId, quantity, selectedSize, customNotes, buttonElement) {
    const originalText = $(buttonElement).html();
    $(buttonElement).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> جاري الإضافة...').prop('disabled', true);

    const payload = {
        ProductId: parseInt(productId),
        Quantity: parseInt(quantity || 1),
        SelectedSize: selectedSize || '',
        CustomNotes: customNotes || ''
    };

    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (response) {
            $(buttonElement).html(originalText).prop('disabled', false);
            if (response.success) {
                showToast('نجاح', response.message, 'success');
                updateCartBadge();
            } else {
                showToast('تنبيه', response.message, 'danger');
            }
        },
        error: function () {
            $(buttonElement).html(originalText).prop('disabled', false);
            showToast('خطأ', 'حدث خطأ أثناء محاولة الإضافة للسلة', 'danger');
        }
    });
}

function showToast(title, message, type) {
    const toastEl = document.getElementById('liveToast');
    if (toastEl) {
        $('#toastTitle').text(title);
        $('#toastBody').text(message);
        $('#liveToast').removeClass('bg-success bg-danger text-white');
        if (type === 'success') {
            $('#liveToast').addClass('bg-success text-white');
        } else {
            $('#liveToast').addClass('bg-danger text-white');
        }
        const toast = new bootstrap.Toast(toastEl);
        toast.show();
    } else {
        alert(message);
    }
}

$(document).ready(function () {
    updateCartBadge();
});
