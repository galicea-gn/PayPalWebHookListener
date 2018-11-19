FROM library/nginx:mainline
ENV PAYPAL_HOST "localhost"
COPY nginx.conf /etc/nginx/nginx.conf
COPY proxy.conf /etc/nginx/proxy.conf
COPY PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.crt /etc/ssl/certs/PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.crt
COPY PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.key /etc/ssl/certs/PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.key
RUN apt-get update && \
    apt-get install sudo -y
ENTRYPOINT ["/bin/sh", "-c", "echo \"PayPal Listener Host: $PAYPAL_HOST\"; sed -i \"s/{{PAYPAL_HOST}}/$PAYPAL_HOST/g\" /etc/nginx/nginx.conf; sudo nginx -g 'daemon off;'"]