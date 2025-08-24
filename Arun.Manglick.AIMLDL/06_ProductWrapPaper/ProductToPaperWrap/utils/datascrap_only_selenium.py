from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
from bs4 import BeautifulSoup
import re
import time

# Set up Chrome options for headless mode and reliability
options = Options()
options.add_argument("--disable-gpu")
options.add_argument("--window-size=1920,1080")
options.add_argument("--lang=en-US")
options.add_argument("--no-sandbox")
options.add_argument("--disable-dev-shm-usage")
options.add_argument("--remote-debugging-port=9222")

# Path to your downloaded ChromeDriver binary
chromedriver_path = r"C:\Users\Arun.Manglick\Downloads\chromedriver-win64\chromedriver-win64\chromedriver.exe"

# Create driver using specific binary
service = Service(executable_path=chromedriver_path)
driver = webdriver.Chrome(service=service, options=options)

# === CONFIGURATION ===
product_urls = [
    "https://www.amazon.com/dp/B07VGRJDFY?th=1",
    "https://www.amazon.com/dp/B09G3HRMVB",  # Add more ASINs or URLs here
]
image_folder = "amazon_images"
output_csv = "amazon_product_data.csv"
# ----------------------------------

# # Target Amazon product URL
# url = "https://www.amazon.com/dp/B0862269YP?th=1"
# driver.get(url)
# time.sleep(3)  # Wait for page to load

# # Get page source and parse with BeautifulSoup
# soup = BeautifulSoup(driver.page_source, "html.parser")

# # === 1. Extract Product Image URL ===
# image_url = "Not found"
# scripts = soup.find_all("script")
# for script in scripts:
#     if script.string and "ImageBlockATF" in script.string:
#         match = re.search(r'"hiRes"\s*:\s*"([^"]+\.jpg)"', script.string)
#         if match:
#             image_url = match.group(1)
#             break

# # === 2. Extract Dimensions from Product Details ===
# height = width = length = "Not found"
# detail_section = soup.find(id="productDetails_techSpec_section_1") or soup.find(id="prodDetails")
# if detail_section:
#     rows = detail_section.find_all("tr")
#     for row in rows:
#         header = row.find("th")
#         value = row.find("td")
#         if header and value:
#             label = header.get_text(strip=True).lower()
#             val = value.get_text(strip=True)
#             if "height" in label:
#                 height = val
#             elif "width" in label:
#                 width = val
#             elif "length" in label or "depth" in label:
#                 length = val

# # === Output ===
# print("✅ Scraped Product Details:")
# print(f"Image URL: {image_url}")
# print(f"Height: {height}")
# print(f"Width: {width}")
# print(f"Length: {length}")
# ----------------------------------
# === SCRAPE LOOP ===
results = []

for url in product_urls:
    driver.get(url)
    time.sleep(3)

    # Title
    try:
        title = driver.find_element(By.ID, "productTitle").text.strip()
    except:
        title = "Not found"

    # Image URL
    try:
        image_element = driver.find_element(By.ID, "landingImage")
        image_url = image_element.get_attribute("src")
    except:
        image_url = "Not found"

    # Dimensions
    height = width = length = "Not found"
    try:
        rows = driver.find_elements(By.XPATH, "//table//tr")
        for row in rows:
            try:
                label = row.find_element(By.TAG_NAME, "th").text.lower()
                value = row.find_element(By.TAG_NAME, "td").text.strip()
                if "height" in label:
                    height = value
                elif "width" in label:
                    width = value
                elif "length" in label or "depth" in label:
                    length = value
            except:
                continue
    except:
        pass

    # === Output ===
    print("✅ Scraped Product Details:")
    print(f"Image URL: {image_url}")
    print(f"Height: {height}")
    print(f"Width: {width}")
    print(f"Length: {length}")

#     # Download image
#     image_filename = "not_downloaded"
#     if image_url != "Not found":
#         try:
#             image_filename = os.path.join(image_folder, title[:50].replace(" ", "_") + ".jpg")
#             img_data = requests.get(image_url).content
#             with open(image_filename, "wb") as f:
#                 f.write(img_data)
#         except:
#             image_filename = "download_failed"

#     # Append result
#     results.append({
#         "title": title,
#         "url": url,
#         "image_url": image_url,
#         "image_file": image_filename,
#         "height": height,
#         "width": width,
#         "length": length
#     })

# # === SAVE TO CSV ===
# df = pd.DataFrame(results)
# df.to_csv(output_csv, index=False)
# print(f"✅ Scraped {len(df)} products. Data saved to {output_csv}")

# ----------------------------------

driver.quit()

