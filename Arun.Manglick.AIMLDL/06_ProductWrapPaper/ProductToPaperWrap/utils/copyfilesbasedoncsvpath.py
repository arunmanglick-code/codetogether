import os
import shutil
import pandas as pd

# === CONFIGURATION ===
csv_path = "raw_product_dataset.csv"              # CSV file containing file paths or names
source_dir = "archive_source"            # Directory to search for files
destination_dir = "archive_dest"  # Where to copy matched files

# Create destination folder if it doesn't exist
os.makedirs(destination_dir, exist_ok=True)

# Load CSV
df = pd.read_csv(csv_path)

# Assume the column containing file names is called 'imagepath'
if 'imagepath' not in df.columns:
    raise ValueError("CSV must contain a column named 'imagepath'")

# Loop through each file name
for file_name in df['imagepath']:
    # Search for the file in source_dir
    for root, dirs, files in os.walk(source_dir):
        if file_name in files:
            source_path = os.path.join(root, file_name)
            destination_path = os.path.join(destination_dir, file_name)

            # Copy the file
            shutil.copy2(source_path, destination_path)
            print(f"✅ Copied: {file_name}")
            break
    else:
        print(f"❌ Not found: {file_name}")
