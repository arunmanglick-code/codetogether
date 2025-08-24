# Step 3: From Dimensions to Wrapping Paper - The Calculation   

# This part of the program is focused on the practical application of the model's output.
# It will involve loading the previously trained model (02_imagedimensionestimationmodel.ipynb), 
# Then using it to predict dimensions of a new product image, and 
# Then feeding those dimensions into a function that calculates the necessary gift wrap paper area.

# def calculate_wrapping_paper_area(dimensions):
#     """
#     Calculate the amount of wrapping paper needed for a product based on its dimensions.
#     """
#     length = dimensions['length_cm']
#     width = dimensions['width_cm']
#     height = dimensions['height_cm']

#     # Calculate the surface area of the box
#     surface_area = 2 * (length * width + width * height + height * length)
#     return surface_area

import numpy as np
import tensorflow as tf
from PIL import Image
import os

# --- 1. Configuration and Model Loading ---

# Configuration
MODEL_PATH = "02_image_dimension_estimation_model.h5"
IMAGE_DIMS = (224, 224, 3) # Must be the same as used during preprocessing (01_imagemassgeneration.ipynb) and training (02_imagedimensionestimationmodel.ipynb)

# Load the trained model
print(f"Loading model from: {MODEL_PATH}")
try:
    model = tf.keras.models.load_model(MODEL_PATH)
    print("Model loaded successfully.")
except (IOError, ImportError) as e:
    print(f"Error loading model: {e}")
    print("Please ensure 'product_dimension_estimator.h5' exists and was generated in Step 2.")
    exit()

# --- 2. Image Prediction Function ---

def predict_dimensions(image_path, model_to_use):
    """
    Takes an image path, preprocesses it, and predicts its dimensions using the loaded model.

    Args:
        image_path (str): The file path to the product image.
        model_to_use (tf.keras.Model): The loaded Keras model.

    Returns:
        dict: A dictionary containing the predicted 'length_cm', 'width_cm', and 'height_cm'.
              Returns None if an error occurs.
    """
    try:
        # Preprocess the new image in the same way as the training data
        img = Image.open(image_path).convert('RGB')
        resized_img = img.resize((IMAGE_DIMS[0], IMAGE_DIMS[1]))
        normalized_array = np.array(resized_img) / 255.0

        # The model expects a batch of images, so we add an extra dimension
        input_data = np.expand_dims(normalized_array, axis=0)

        # Make the prediction
        predicted_dims = model_to_use.predict(input_data)[0]

        # Ensure dimensions are not negative (a possible, though unlikely, model artifact)
        predicted_dims[predicted_dims < 0] = 0

        return {
            "length_cm": predicted_dims[0],
            "width_cm": predicted_dims[1],
            "height_cm": predicted_dims[2]
        }
    except FileNotFoundError:
        print(f"Error: Image not found at {image_path}")
        return None
    except Exception as e:
        print(f"An error occurred during prediction: {e}")
        return None


# --- 3. Gift Wrap Calculation Function ---

def calculate_wrapping_paper_size(dimensions, overlap_cm=5.0):
    """
    Calculates the required gift wrap paper size for a rectangular product.

    This formula ensures enough paper to wrap around the item's longest circumference
    and to cover its ends.

    Args:
        dimensions (dict): A dictionary with 'length_cm', 'width_cm', 'height_cm'.
        overlap_cm (float): Extra buffer for taping and adjustments.

    Returns:
        dict: A dictionary with the required 'paper_length_cm' and 'paper_width_cm'.
    """
    # Sort dimensions to handle the product in any orientation
    # l > w > h
    dims = sorted([dimensions['length_cm'], dimensions['width_cm'], dimensions['height_cm']], reverse=True)
    length, width, height = dims[0], dims[1], dims[2]

    # Formula 1: A common method found on gift wrapping guides
    # Paper Length = Circumference of the shorter side + overlap
    # Paper Width = Length of box + Height of box
    paper_length = (width * 2) + (height * 2) + overlap_cm
    paper_width = length + height + (overlap_cm / 2) # A smaller overlap for the ends

    # Formula 2: Surface Area (less practical for actual wrapping but good for a rough estimate)
    # surface_area = 2 * (length * width + width * height + height * length)

    return {
        "paper_length_cm": paper_length,
        "paper_width_cm": paper_width
    }

# --- 4. Main Program Execution ---

def main(image_path):
    """
    Main function to run the complete workflow.
    """
    print("\n" + "="*50)
    print(f"Processing image: {os.path.basename(image_path)}")
    print("="*50) # print a horizontal divider line made of 50 equal signs (=).

    # Step A: Get dimensions from the image
    predicted_dimensions = predict_dimensions(image_path, model)

    if predicted_dimensions:
        print("\n[STEP 1] Predicted Product Dimensions:")
        print(f"  - Length: {predicted_dimensions['length_cm']:.2f} cm")
        print(f"  - Width:  {predicted_dimensions['width_cm']:.2f} cm")
        print(f"  - Height: {predicted_dimensions['height_cm']:.2f} cm")

    # Step B: Calculate the required paper size
        paper_size = calculate_wrapping_paper_size(predicted_dimensions)

        print("\n[STEP 2] Recommended Gift Wrap Paper Size:")
        print(f"  - You will need a sheet of paper that is at least:")
        print(f"    -> {paper_size['paper_length_cm']:.2f} cm long")
        print(f"    -> {paper_size['paper_width_cm']:.2f} cm wide")
        print("\nThis includes a small buffer for overlap and taping.")
        print("="*50)

if __name__ == "__main__":
    # --- Provide the path to a random product image here ---
    # To test, you can use an image from your original dataset folder,
    # or any other product image you have - 'image_to_predict/airplane_clock_4.jpg'

    # Create a dummy image for testing if none is available
    producttopredictimagepath = 'image_to_predict/81b93e5d.jpg'

    if not os.path.exists(producttopredictimagepath):
        print("Creating a dummy test image: 'test_product.jpg'")
        try:
            dummy_image = Image.new('RGB', (300, 300), color = 'red')
            dummy_image.save('test_product.jpg')
            test_image_path = "test_product.jpg"
        except Exception as e:
            print(f"Could not create a dummy image: {e}")
            test_image_path = "" # Will cause an error below, handled gracefully
    else:
        test_image_path = producttopredictimagepath


    if test_image_path:
        main(test_image_path)
    else:
        print("\nPlease set a valid path for 'test_image_path' to run the program.")