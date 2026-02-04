from xhtml2pdf import pisa
import sys

def convert_html_to_pdf(source_html, output_filename):
    # open output file for writing (truncated binary)
    result_file = open(output_filename, "w+b")

    # convert HTML to PDF
    pisa_status = pisa.CreatePDF(
            source_html,                # the HTML to be converted
            dest=result_file)           # file handle to recieve result

    # close output file
    result_file.close()                 # close output file

    # return True on success and False on errors
    return pisa_status.err

if __name__ == "__main__":
    html_file = sys.argv[1]
    pdf_file = sys.argv[2]
    with open(html_file, "r", encoding="utf-8") as f:
        html_content = f.read()
    err = convert_html_to_pdf(html_content, pdf_file)
    if not err:
        print("PDF created successfully")
    else:
        print("Error creating PDF")
