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

# Target Amazon product URL
url = "https://www.amazon.com/dp/B0862269YP?th=1"
driver.get(url)
time.sleep(3)  # Wait for page to load

# Get page source and parse with BeautifulSoup
soup = BeautifulSoup(driver.page_source, "html.parser")

# === 1. Extract Product Image URL ===
image_url = "Not found"
scripts = soup.find_all("script")
for script in scripts:
    if script.string and "ImageBlockATF" in script.string:
        match = re.search(r'"hiRes"\s*:\s*"([^"]+\.jpg)"', script.string)
        if match:
            image_url = match.group(1)
            break

# === 2. Extract Dimensions from Product Details ===
height = width = length = "Not found"
detail_section = soup.find(id="productDetails_techSpec_section_1") or soup.find(id="prodDetails")
if detail_section:
    rows = detail_section.find_all("tr")
    for row in rows:
        header = row.find("th")
        value = row.find("td")
        if header and value:
            label = header.get_text(strip=True).lower()
            val = value.get_text(strip=True)
            if "height" in label:
                height = val
            elif "width" in label:
                width = val
            elif "length" in label or "depth" in label:
                length = val

# === Output ===
print("✅ Scraped Product Details:")
print(f"Image URL: {image_url}")
print(f"Height: {height}")
print(f"Width: {width}")
print(f"Length: {length}")

driver.quit()

