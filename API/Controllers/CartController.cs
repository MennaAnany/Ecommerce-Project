using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]

        public async Task<ActionResult<CartDto>> GetCart()
        {
            var cart = await _unitOfWork.CartRepository.GetUserCartAsync(User.GetUserId());
            return cart == null ? NotFound("Cart not found for this user") : cart;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CartDto>> AddItemToCart(AddCartItemDto itemDto)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(User.GetUserId());

            if (cart == null) return NotFound("Cart not found for this user");


            var item = _unitOfWork.CartRepository.GetCartItems(cart, itemDto);

            if (item == null)
            {
                var newItem = new CartItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = 1,
                    Color = itemDto.Color,
                    Size = itemDto.Size
                };

                cart.CartItems.Add(newItem);
            }
            else
            {
                item.Quantity++;
            }

            _unitOfWork.CartRepository.UpdateCart(cart);

            if (await _unitOfWork.Complete())
            {
                return _mapper.Map<CartDto>(cart);
            }
            return BadRequest("This item cannot be added.");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(User.GetUserId());

            if (cart == null) return NotFound("Cart not found for this user");


            var item = _unitOfWork.CartRepository.GetCartItem(cart, id);

            if (item == null) return BadRequest("Your cart doesn't contain this item!");


            _unitOfWork.CartRepository.DeleteCartItem(cart, item);

            _unitOfWork.CartRepository.UpdateCart(cart);

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }
            return BadRequest("Unable to delete this item");
        }

        [Authorize]
        [HttpDelete]

        public async Task<ActionResult> DeleteCart()
        {
            var userId = User.GetUserId();
            var cart = await _unitOfWork.CartRepository.GetCartAsync(userId);

            if (cart == null) return NotFound("Cart not found for this user");

            cart.CartItems.Clear();
            cart.SubTotal = 0;

            _unitOfWork.CartRepository.UpdateCart(cart);
            await _unitOfWork.Complete();

           return NoContent();
        }
    }
}