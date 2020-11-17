$(document).ready(function () {
  function incrementValue(e) {
    e.preventDefault();
    const fieldName = $(e.target).data('field');
    const parent = $(e.target).closest('div');
    const currentVal = parseInt(parent.find('input[name=' + fieldName + ']').val(), 10);

    if (!isNaN(currentVal)) {
      parent.find('input[name=' + fieldName + ']').val(currentVal + 1);
    } else {
      parent.find('input[name=' + fieldName + ']').val(0);
    }
  }

  function decrementValue(e) {
    e.preventDefault();
    const fieldName = $(e.target).data('field');
    const parent = $(e.target).closest('div');
    const currentVal = parseInt(parent.find('input[name=' + fieldName + ']').val(), 10);

    if (!isNaN(currentVal) && currentVal > 0) {
      parent.find('input[name=' + fieldName + ']').val(currentVal - 1);
    } else {
      parent.find('input[name=' + fieldName + ']').val(0);
    }
  }

  $('.input-group').on('click', '.button-plus', function(e) {
    incrementValue(e);
  });

  $('.input-group').on('click', '.button-minus', function(e) {
    decrementValue(e);
  });
  
  $('#sort').on('change', function () {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    
    let order;
    switch (this.value) {
      case '1':
        order = 'highestRating';
        break;
      case '2':
        order = 'lowestRating';
        break;
      case '3':
        order = 'highestPrice';
        break;
      case '4':
        order = 'lowestPrice';
        break;
      default:
        order = 'highestRating';
    }
    
    let redirectUrl = urlParams.has('id') ? '/category?id=' + urlParams.get('id') + '&order=' + order + '&page=1' : 
        '/search?query=' + urlParams.get('query') + '&order=' + order + '&page=1';
    
    window.location.replace(redirectUrl);
  });
  
  $('#add-to-cart').on('click', function () {
    $('#add-to-cart').off('click');
    let cartStr = window.sessionStorage.getItem("cart");
    let cart = (cartStr != null) ? JSON.parse(cartStr) : [];
    let productId = $('#product-id').val();

    let i;
    for (i = 0; i < $('#count').val(); i++) {
      cart.push(productId);
    }
    
    $.ajax({
      type: 'POST',
      dataType: 'text',
      url: '/cart/setcart',
      data: {
        jArray: JSON.stringify(cart)
      },
      success: function (result) {
        window.location.replace('/cart');
      }
    });
    
  });
  
  $('#make-order').on('click', function () {
    $('#make-order').off('click');
    
    $.ajax({
      type: 'POST',
      dataType: 'text',
      url: '/cart/makeorder',
      data: {},
      success: function (result) {
        window.location.replace('/cart/success');
      }
    });
  });
});
