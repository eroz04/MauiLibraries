// ReSharper disable all
namespace Openpay;

public enum CardInfoErrorField
{
    None,
    Titular,
    CardNumber,
    ExpirationMonth,
    ExpirationYear,
    ExpirationDate,
    SecuriryCode
}

public class CardInfoErrorMessages
{
    public static string GetErrorDescription(int code, string defaultDescription, out CardInfoErrorField field)
    {
        field = CardInfoErrorField.None;
        string description = "";
        switch (code)
        {
            case 1000:
                description = "Ocurrió un error interno en el servidor de Openpay";
                break;
            case 1002:
                description = "La llamada no esta autenticada o la autenticación es incorrecta.";
                break;
            case 1003:
                description = "La operación no se pudo completar por que el valor de uno o más de los parámetros no es correcto.";
                break;
            case 1004:
                description = "Un servicio necesario para el procesamiento de la transacción no se encuentra disponible.";
                break;
            case 1005:
                description = "Uno de los recursos requeridos no existe.";
                break;
            case 1006:
                description = "Ya existe una transacción con el mismo ID de orden.";
                break;
            case 1009:
                description = "El cuerpo de la petición es demasiado grande.";
                break;
            case 1011:
                description = "Se solicita un recurso que esta marcado como eliminado.";
                break;
            case 1014:
                description = "La cuenta esta inactiva.";
                break;
            case 1015:
                description = "No se ha obtenido respuesta de la solicitud realizada al servicio.";
                break;
            case 1001:
                field = CardInfoErrorField.SecuriryCode;
                if (defaultDescription.Contains("must be 4 digits"))
                    description = "CVV debe ser de 4 dígitos";
                else if (defaultDescription.Contains("must be 3 digits"))
                    description = "CVV debe ser de 3 dígitos";
                else
                    description = "CVV debe ser de 3 dígitos";
                break;
            case 2004:
                field = CardInfoErrorField.CardNumber;
                description = "El número de tarjeta es inválido.";
                break;
            case 2005:
                field = CardInfoErrorField.ExpirationDate;
                description = "La fecha de expiración de la tarjeta es anterior a la fecha actual.";
                break;
            case 2006:
                field = CardInfoErrorField.SecuriryCode;
                description = "El código de seguridad de la tarjeta (CVV2) no fue proporcionado.";
                break;
            case 2007:
                field = CardInfoErrorField.CardNumber;
                description = "El número de tarjeta es de prueba, solamente puede usarse en Sandbox.";
                break;
            case 2008:
                field = CardInfoErrorField.CardNumber;
                description = "El número de tarjeta es de prueba, solamente puede usarse en Sandbox.";
                break;
            case 2009:
                field = CardInfoErrorField.SecuriryCode;
                description = "El código de seguridad de la tarjeta CVV es inválido.";
                break;
            case 2010:
                description = "Autenticación 3D Secure fallida.";
                break;
            case 2011:
                description = "Tipo de tarjeta no soportada.";
                break;
            case 3001:
                description = "La tarjeta fue declinada por el banco.";
                break;
            case 3002:
                description = "La tarjeta ha expirado.";
                break;
            case 3003:
                description = "La tarjeta no tiene fondos suficientes.";
                break;
            case 3004:
                description = "La tarjeta ha sido identificada como una tarjeta robada.";
                break;
            case 3005:
                description = "La tarjeta ha sido rechazada por el sistema antifraude. -- Rechazada por coincidir con registros en lista negra.";
                break;
            case 3006:
                description = "La operación no esta permitida para este cliente o esta transacción.";
                break;
            case 3009:
                description = "La tarjeta fue reportada como perdida.";
                break;
            case 3010:
                description = "El banco ha restringido la tarjeta.";
                break;
            case 3011:
                description = "El banco ha solicitado que la tarjeta sea retenida. Contacte al banco.";
                break;
            case 3012:
                description = "Se requiere solicitar al banco autorización para realizar este pago.";
                break;
            case 3201:
                description = "Comercio no autorizado para procesar pago a meses sin intereses.";
                break;
            case 3203:
                description = "Promoción no valida para este tipo de tarjetas.";
                break;
            case 3204:
                description = "El monto de la transacción es menor al mínimo permitido para la promoción.";
                break;
            case 3205:
                description = "Promoción no permitida.";
                break;
            default:
                description = defaultDescription;
                break;
        }
        return description;
    }
}
