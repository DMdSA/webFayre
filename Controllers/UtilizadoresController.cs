using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFayre.Models;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace WebFayre.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly WebFayreContext _context;
        // NOTE: You should use a private-key that's a LOT longer than just 4 bytes.
        private static readonly Byte[] _privateKey = new byte[] { 0xDA, 0xCF, 0xD3, 0x75 };
        private static readonly TimeSpan _passwordResetExpiry = TimeSpan.FromMinutes(5);
        private const Byte _version = 1; // increment this whenever the structure of the message changes

        public UtilizadoresController(WebFayreContext context)
        {
            _context = context;
        }

        // GET: Utilizadors
        public async Task<IActionResult> Index()
        {
              return _context.Utilizadors != null ? 
                          View(await _context.Utilizadors.ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.Utilizadors'  is null.");
        }

        // GET: Utilizadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utilizadors == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (Email == null || Password == null || _context.Utilizadors == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors
                .FirstOrDefaultAsync(m => m.Email == Email && m.Password == Password);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View("LoginSuccess", utilizador);
        }


        // GET: Utilizadors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Password,Rua,Porta,CodigoPostal,Telemovel,Nif,DataNascimento,UtilizadorPath")] Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utilizadors == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Password,Rua,Porta,CodigoPostal,Telemovel,Nif,DataNascimento,UtilizadorPath")] Utilizador utilizador)
        {
            if (id != utilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utilizadors == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utilizadors == null)
            {
                return Problem("Entity set 'WebFayreContext.Utilizadors'  is null.");
            }
            var utilizador = await _context.Utilizadors.FindAsync(id);
            if (utilizador != null)
            {
                _context.Utilizadors.Remove(utilizador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
          return (_context.Utilizadors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /**
         * CreatePasswordResetHmacCode
         * 
         * sets up a reset password code
         * 
         * */
        public static String CreatePasswordResetHmacCode(Int32 userId)
        {
            // message constructor
            Byte[] message = Enumerable.Empty<Byte>()
                .Append(_version)                                           // 1 byte
                .Concat(BitConverter.GetBytes(userId))                      // int size (4)
                .Concat(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()))
                .ToArray();

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                Byte[] hash = hmacSha256.ComputeHash(buffer: message, offset: 0, count: message.Length);

                Byte[] outputMessage = message.Concat(hash).ToArray();
                String outputCodeB64 = Convert.ToBase64String(outputMessage);
                String outputCode = outputCodeB64.Replace('+', '-').Replace('/', '_');
                return outputCode;
            }
        }


        public static Boolean VerifyPasswordResetHmacCode(String codeBase64Url, out Int32 userId)
        {
            userId = 0;
            string base64 = codeBase64Url.Replace('-', '+').Replace('_', '/');
            byte[] message = Convert.FromBase64String(base64);

            byte version = message[0];
            if (version < _version) return false;

            userId = BitConverter.ToInt32(message, startIndex: 1); // Reads bytes message[1,2,3,4]
            long createdUtcBinary = BitConverter.ToInt64(message, startIndex: 1 + sizeof(int)); // Reads bytes message[5,6,7,8,9,10,11,12]

            DateTime createdUtc = DateTime.FromBinary(createdUtcBinary);
            // if expiration date ends, return false
            if (createdUtc.Add(_passwordResetExpiry) < DateTime.UtcNow) return false;

            // (version) + (id) + (expiration_date)
            const int _messageLength = 1 + sizeof(int) + sizeof(long); // 1 + 4 + 8 == 13

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(message, offset: 0, count: _messageLength);

                byte[] messageHash = message.Skip(_messageLength).ToArray();
                return Enumerable.SequenceEqual(hash, messageHash);
            }
        }

        
        // Note there is no `UserId` URL parameter anymore because it's embedded in `code`:

        [HttpGet("/PasswordReset/ResetPassword/{codeBase64Url}")]
        public IActionResult ConfirmResetPassword(String codeBase64Url)
        {

            Console.WriteLine(codeBase64Url);

            if (!VerifyPasswordResetHmacCode(codeBase64Url, out Int32 userId))
            {
                // Message is invalid, such as the HMAC hash being incorrect, or the code has expired.
                return this.BadRequest("Invalid, tampered, or expired code used.");
            }
            else
            {

                return View();
                // Return a web-page with a <form> to POST the code.
                // Render the `codeBase64Url` to an <input type="hidden" /> to avoid the user inadvertently altering it.
                // Do not reset the user's password in a GET request because GET requests must be "safe". If you send a password-reset link by SMS text message or even by email, then software bot (like link-preview generators) may follow the link and inadvertently reset the user's password!
            }
        }

        
        [HttpPost("/PasswordReset/ResetPassword/{codeBase64Url}")]
        public IActionResult ConfirmResetPassword([FromForm]ConfirmResetPasswordForm model)
        {

            if (model.newPassword != model.newPasswordConfirmation)
            {
                ViewBag.passwordsMatch = "Your passwords aren't matching!!";
                return View();
            }

            if (!VerifyPasswordResetHmacCode(model.CodeBase64Url, out Int32 userId))
            {
                return this.BadRequest("Invalid, tampered, or expired code used.");
            }
            
            else
            {
                // Reset the user's password here.
                var utilizador = _context.Utilizadors.SingleOrDefault(u => u.Id == userId);
                if (utilizador != null)
                {
                    utilizador.Password = model.newPassword;
                    _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Home");
                }
                else return NotFound();
            }

        }
        


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {

            var user = _context.Utilizadors
                .SingleOrDefault(u => u.Email == email);

            if (user != null)
            {
                var url_code = CreatePasswordResetHmacCode(user.Id);
                //Console.WriteLine(url_code);
                // send to email
                await sendVerificationLinkEmail(email, url_code);
            }

            ViewBag.forgot = "Email sent! Check it to change your password";

            return View();
        }


        public async Task sendVerificationLinkEmail(string email, string codeBase64Url)
        {
            var verifyUrl = "/PasswordReset/ResetPassword/" + codeBase64Url;
            var link = $"{Request.Scheme}://{Request.Host}/PasswordReset/ResetPassword/" + codeBase64Url;

            var fromEmail = new MailAddress("webfayre.help@gmail.com", "WebFayre Support");

            // activate your email "app sign in password" for this to work! 
            var fromEmailPassword = "xivjuynwylcudhmj";
            var toEmail = new MailAddress(email);

            string subject = "WebFayre - Request for password change";
            string body = "<br/><br/>Here is a link for you to define your new password.<br/>Greetings, <br/>hope you're having the " +
                "best of the days,<br/>WebFayre." +
                "<br/><br/><a href='" + link + "'>" + "Change Password" + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        public async Task<IActionResult> FairHistory()
        {
            if (HttpContext.Session.GetInt32("utilizadorId") == null)
            {
                return RedirectToAction("login", "home");
            }
            // get current user's id
            var userid = (int)HttpContext.Session.GetInt32("utilizadorId");

            // get all tickets generated for him
            var tickets = _context.Tickets != null ? await _context.Tickets.Where(t => t.UtilizadorId == userid).Select(t => t.FeiraId).ToListAsync() : null;

            return _context.Feiras != null ?
                          View(await _context.Feiras.Where(f => tickets.Contains(f.IdFeira)).ToListAsync()) :
                          Problem("Entity set 'WebFayreContext.Feiras'  is null.");
        }


    }


}
