import requests
from bs4 import BeautifulSoup
import pandas as pd
import os
import re

def scrape_product_data(url):
    """
    Scrapes product data from a given URL.
    NOTE: The selectors used here are examples for an Amazon product page
          and will likely need to be updated for other pages or websites.
    """
    headers = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
    }

    try:
        response = requests.get(url, headers=headers)
        response.raise_for_status()  # Raise an exception for bad status codes
    except requests.exceptions.RequestException as e:
        print(f"Error fetching URL {url}: {e}")
        return None

    soup = BeautifulSoup(response.content, 'html.parser')

    # === 1. MS CoPilot - Extract Product Image URL ===
    image_url = "Not found"
    scripts = soup.find_all("script")
    print(f"Found {len(scripts)} script tags.")

    for script in scripts:
        print(f"Script content (first 1000 chars): {script.string[:1000] if script.string else 'No content'}")
        if script.string and "ImageBlockATF" in script.string:
            match = re.search(r'"hiRes"\s*:\s*"([^"]+\.jpg)"', script.string)
            if match:
                image_url = match.group(1)
                print(f"Found image URL: {image_url}")
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

    # -----------------------

    # --- These selectors need to be adapted for the specific product page ---
    # Example for Product Image URL
    # image_element = soup.select_one('img#landingImage')
    # image_url = image_element['src'] if image_element else 'Not Found'

    # # Example for Product Dimensions (often found in a details table)
    # dimensions_element = None
    # # Amazon often has dimensions in a table or a list
    # # We will search for text containing "Product Dimensions" or similar
    # product_details_tables = soup.find_all('table', id=re.compile(r'productDetails'))
    
    # height, width, length = 'Not Found', 'Not Found', 'Not Found'

    # print('product_details_tables:', len(product_details_tables))

    # for table in product_details_tables:
    #     rows = table.find_all('tr')
    #     for row in rows:
    #         header = row.find('th')
    #         if header and 'Product Dimensions' in header.get_text():
    #             dimensions_text = row.find('td').get_text(strip=True)
    #             # This regex will need to be adjusted based on the format of the dimensions
    #             dims = re.findall(r'(\d+\.?\d*)', dimensions_text)
    #             if len(dims) >= 3:
    #                 length, width, height = dims[0], dims[1], dims[2]
    #             break
    #     if length != 'Not Found':
    #         break

    # --- End of adaptable selectors ---

    product_data = {
        'image_url': image_url,
        'height_cm': height,
        'width_cm': width,
        'length_cm': length,
        'product_url': url
    }

    return product_data

if __name__ == '__main__':
    # --- List of product URLs to scrape ---
    product_urls = [
        # Replace these with the URLs of the products you want to scrape
        'https://www.amazon.com/dp/B0862269YP'
    ]

    scraped_data = []
    print("Starting product scraping...")
    for url in product_urls:
        data = scrape_product_data(url)
        if data:
            scraped_data.append(data)
            print(f"Successfully scraped: {url}")
        else:
            print(f"Failed to scrape: {url}")

    # Convert the scraped data into a DataFrame for easier analysis
    df = pd.DataFrame(scraped_data)
    print("Scraping complete. Here's the data collected:")
    print(df)