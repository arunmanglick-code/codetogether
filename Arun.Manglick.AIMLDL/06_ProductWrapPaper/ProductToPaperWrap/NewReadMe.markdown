# Project: Product to Paper Wrap

## Step 0: Collect Data (Product Images and CSV)
- **Option 1: Scraping (Note: Scraping may have legal implications)**
  - Using BeautifulSoup Library (`\utils\datascraping.py`)
  - Using BeautifulSoup + Selenium (`\utils\datascrap_bs_selenium.py`)
  - Using Only Selenium (`\utils\datascrap_only_selenium.py`)
- **Option 2: (Used Here)**
  - Use Datasource to download the dataset
  - Dataset Used: Amazon Berkeley Objects Dataset
    - Source: [Amazon Berkeley Objects](https://amazon-berkeley-objects.s3.amazonaws.com/index.html)
    - Downloaded: `abo-images-small.tar` — Downscaled (max 256 pixels) catalog images and metadata (3 GB)
  - Downloaded all product images and the associated CSV containing image URLs and dimensions (Height/Width/Length)
  - Dataset contains 32K records; used ~5% (approximately 1500 product images) for this project
  - Copied ~1500 product images to folder: `\utils\archive_source`
  - **Data Cleanup (Highlights)**
    - Dimensions were in various units (inches, cm, etc.), and many rows lacked dimension data
    - Applied Machine Learning concept 'Exploratory Analysis':
      a. Imputing Missing Data
      b. Handling Unbalanced Data
    - Output: Reduced 1500 rows to ~150 rows in the dataset CSV
  - **Copying Images**
    - Created utility to copy 150 product images based on CSV paths: `\utils\copyfilesbasedoncsvpath.py`
    - Copied 150 product images to folder: `\utils\archive_dest`

## Step 1: Data Preprocessing
- Copied 150 images from `\utils\archive_dest` to `\ProductToPaperWrap\input_raw_images`
- Copied the CSV (`raw_product_dataset.csv`) with 150 rows from `\utils` to `\ProductToPaperWrap\input_raw_images`
- Added Jupyter Notebook: `\ProductToPaperWrap\01_imagemassgeneration.ipynb`
- **Scope**
  - Preprocess images to a consistent format for the machine learning model
  - Key Tasks:
    1. **Resizing**: All images resized to the same dimensions
    2. **Normalization**: Pixel values scaled to a range of [0, 1]
    3. **Augmentation**: Skipped here (e.g., rotation, flipping)
  - Applied: Resizing and Normalization
- **Output**
  - Jupyter Notebook generates resized and normalized images (saved as `.npy` files)
  - Stored at: `\ProductToPaperWrap\preprocessed_raw_images`
- **Execution Output**
  ```
  Preprocessing images from 'input_raw_images'...
  Preprocessing complete. Processed images and arrays are in 'preprocessed_raw_images'
  ```

## Step 2: Building the Core: A Deep Learning Model for Dimension Estimation
- **Scope**: Build, train, evaluate, compile, and save a model to predict product dimensions (length, width, height) from images
- **Highlights**
  - Used **MobileNetV2** architecture (TensorFlow and Keras) for efficiency and performance
  - Applied **transfer learning** by adapting a pre-trained Convolutional Neural Network (CNN)
- **MobileNetV2**
  - Lightweight CNN designed for mobile and embedded vision applications
  - Uses depthwise separable convolutions to reduce parameters and computations
- **Transfer Learning**
  - Starts with a pre-trained model and fine-tunes for specific tasks
  - Approaches:
    - Continue training a pre-trained model (fine-tuning)
    - Add new trainable layers to a frozen model
    - Retrain from scratch
    - Use as-is
- **Input**
  - CSV: `\ProductToPaperWrap\input_raw_images\raw_product_dataset.csv`
  - Preprocessed images: `\ProductToPaperWrap\preprocessed_raw_images`
- **Output**
  - Generated model: `02_image_dimension_estimation_model.h5`
- **Testing**
  - Tested with a random image: `\ProductToPaperWrap\image_to_predict\81b93e5d.jpg`
  - Test Code:
    ```python
    image_path = "image_to_predict/81b93e5d.jpg"
    predicted_dimensions = predict_product_dimensions(image_path, product_size_prediction_model)
    print(predicted_dimensions)
    ```
  - Test Output:
    ```python
    {'length_cm': np.float32(37.32475), 'width_cm': np.float32(24.118963), 'height_cm': np.float32(27.064758)}
    ```
- **Testing Model Accuracy**
  - Tested with an image from the dataset: `\ProductToPaperWrap\input_raw_images\81a0b666.jpg`
  - Output:
    ```
    Image: input_raw_images\81a0b666.jpg
    Predicted Dimensions -> Length: 104.36 cm, Width: 81.95 cm, Height: 67.58 cm
    Actual Dimensions    -> Length: 91.44 cm, Width: 2.49 cm, Height: 2.21 cm
    ```

## Step 3: From Product Predicted Dimensions to Calculating Wrapping Paper Size
- Implemented as an executable Python script
- **Scope**
  - Load the trained model (`02_image_dimension_estimation_model.h5`)
  - Predict dimensions of a new product image
  - Calculate the required gift wrap paper area
- **Steps Involved**
  - Load the created model
  - Create an image prediction function
  - Create a gift wrap calculation function
  - Main program execution:
    - Accepts an image (e.g., `producttopredictimagepath = 'image_to_predict/81b93e5d.jpg'`)
    - Predicts dimensions using the model
    - Passes dimensions to the gift wrap calculation function
- **How to Run**
  - Navigate to the directory and run:
    ```
    \ProductToPaperWrap > python 03_product_giftwrap_predictor.py
    ```
- **Execution Output**
  ```
  Model loaded successfully.
  ==================================================
  Processing image: 81b93e5d.jpg
  ==================================================
  1/1 ━━━━━━━━━━━━━━━━━━━━ 2s 2s/step

  [STEP 1] Predicted Product Dimensions:
    - Length: 41.78 cm
    - Width:  25.67 cm
    - Height: 27.53 cm

  [STEP 2] Recommended Gift Wrap Paper Size:
    - You will need a sheet of paper that is at least:
      -> 111.41 cm long
      -> 69.95 cm wide

  This includes a small buffer for overlap and taping.
  ==================================================
  ```