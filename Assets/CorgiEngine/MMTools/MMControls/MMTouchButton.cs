using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{	
	[RequireComponent(typeof(Rect))]
	[RequireComponent(typeof(CanvasGroup))]
	/// <summary>
	/// Add this component to a GUI Image to have it act as a button. 
	/// Bind pressed down, pressed continually and released actions to it from the inspector
	/// Handles mouse and multi touch
	/// </summary>
	public class MMTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
	{
		public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp }
		[Header("Binding")]
		/// The method(s) to call when the button gets pressed down
		public UnityEvent ButtonPressedFirstTime;
		/// The method(s) to call when the button gets released
		public UnityEvent ButtonReleased;
		/// The method(s) to call while the button is being pressed
		public UnityEvent ButtonPressed;

		[Header("Pressed Behaviour")]
		[Information("Here you can set the opacity of the button when it's pressed. Useful for visual feedback.",InformationAttribute.InformationType.Info,false)]
		/// the new opacity to apply to the canvas group when the button is pressed
		public float PressedOpacity = 0.5f;

		[Header("Mouse Mode")]
		[Information("If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).",InformationAttribute.InformationType.Info,false)]
		/// If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).
		public bool MouseMode = false;

		/// the current state of the button (off, down, pressed or up)
		public ButtonStates CurrentState { get; protected set; }

	    protected bool _zonePressed = false;
	    protected CanvasGroup _canvasGroup;
	    protected float _initialOpacity;

	    /// <summary>
	    /// On Start, we get our canvasgroup and set our initial alpha
	    /// </summary>
	    protected virtual void Awake()
	    {
			_canvasGroup = GetComponent<CanvasGroup>();
			if (_canvasGroup!=null)
			{
				_initialOpacity = _canvasGroup.alpha;
			}
			ResetButton();
	    }

		/// <summary>
		/// Every frame, if the touch zone is pressed, we trigger the OnPointerPressed method, to detect continuous press
		/// </summary>
		protected virtual void Update()
	    {
			if (CurrentState == ButtonStates.ButtonPressed)
	        {
	            OnPointerPressed();
	        }
	    }

		/// <summary>
		/// At the end of every frame, we change our button's state if needed
		/// </summary>
		protected virtual void LateUpdate()
		{
			if (CurrentState == ButtonStates.ButtonUp)
			{
				CurrentState = ButtonStates.Off;
			}
			if (CurrentState == ButtonStates.ButtonDown)
			{
				CurrentState = ButtonStates.ButtonPressed;
			}
		}

		/// <summary>
		/// Triggers the bound pointer down action
		/// </summary>
		public virtual void OnPointerDown(PointerEventData data)
	    {
			if (CurrentState != ButtonStates.Off)
			{
				return;
			}

			CurrentState = ButtonStates.ButtonDown;
			if (_canvasGroup!=null)
			{
				_canvasGroup.alpha=PressedOpacity;
			}
			if (ButtonPressedFirstTime!=null)
	        {
				ButtonPressedFirstTime.Invoke();
	        }
	    }

		/// <summary>
		/// Triggers the bound pointer up action
		/// </summary>
		public virtual void OnPointerUp(PointerEventData data)
		{
			if (CurrentState != ButtonStates.ButtonPressed && CurrentState != ButtonStates.ButtonDown)
			{
				return;
			}

			CurrentState = ButtonStates.ButtonUp;
			if (_canvasGroup!=null)
			{
				_canvasGroup.alpha=_initialOpacity;
			}
			if (ButtonReleased != null)
			{
				ButtonReleased.Invoke();
	        }
	    }

		/// <summary>
		/// Triggers the bound pointer pressed action
		/// </summary>
		public virtual void OnPointerPressed()
	    {
			CurrentState = ButtonStates.ButtonPressed;
			if (ButtonPressed != null)
			{
				ButtonPressed.Invoke();
	        }
	    }

	    /// <summary>
	    /// OnEnable, we reset our button state
	    /// </summary>
		protected virtual void OnEnable()
	    {
			ResetButton();
	    }

	    /// <summary>
	    /// Resets the button's state and opacity
	    /// </summary>
	    protected virtual void ResetButton()
	    {
			_canvasGroup.alpha = _initialOpacity;
			CurrentState = ButtonStates.Off;
		}

		/// <summary>
		/// Triggers the bound pointer enter action when touch enters zone
		/// </summary>
		public void OnPointerEnter(PointerEventData data)
		{
			if (!MouseMode)
			{
				OnPointerDown (data);
			}
		}

		/// <summary>
		/// Triggers the bound pointer exit action when touch is out of zone
		/// </summary>
		public void OnPointerExit(PointerEventData data)
		{
			if (!MouseMode)
			{
				OnPointerUp(data);	
			}
		}
	}
}
